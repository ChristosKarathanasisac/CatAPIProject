using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using NatechAPI.Data;
using NatechAPI.Models.Entities;
using NatechAPI.Models.ViewModels;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;

namespace NatechAPI.Services
{
    public class CatsService 
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ExternalApiService externalApiService;
        public CatsService(ApplicationDbContext context, ExternalApiService externalApiService)
        {
            this.dbContext = context;
            this.externalApiService = externalApiService;
        }
        public async Task<HashSet<string>> AddCatsToDb() 
        {
            List<CatVM> catVMs = new List<CatVM>();
            catVMs = await GetCatsFromApi();

            if (catVMs == null)
            {
                return null;
            }
            
            HashSet<string> tags = new HashSet<string>();
            HashSet<string> insCats = new HashSet<string>();
            if (!InsertCatsToDbContext(catVMs, ref tags,out insCats)) 
            {
                return null;
            }
            if (!InsertTagsToDbContext(tags)) 
            {
                return null;
            }

            catVMs = catVMs.Where(cat => insCats.Contains(cat.id)).ToList();
            if (catVMs.Count > 0)
            {
                bool success= await InsertRelationsToDb(catVMs);
                if (success)
                {
                    return insCats;
                }
                return null;
            }
            else 
            {
                //Nothing added
                return insCats;
            }
        }
        public async Task<ReturnedCatsVM> GetCatById(string id)
        {
            try
            {
                CatEntity cat = await dbContext.Cats
                                                    .Include(c => c.CatTags)
                                                        .ThenInclude(ct => ct.TagEntity)
                                                    .Where(c => c.CatId.Equals(id.Trim()))
                                                    .FirstOrDefaultAsync();

                if (cat != null)
                {
                    ReturnedCatsVM returnedCatsVM = new ReturnedCatsVM
                    {
                        CatId = cat.CatId,
                        Width = cat.Width,
                        Height = cat.Height,
                        Image = cat.Image,
                        Tags = string.Join(',', cat.CatTags.Select(ct => ct.TagEntity.Name).ToList())
                    };
                    return returnedCatsVM;
                }
                else 
                {
                    return new ReturnedCatsVM();
                }
                
            }
            catch (Exception exc)
            {
                //Some log
                return null;
            }
        }

        public async  Task<GetCatsPegResponseVM> GetCatsWithPegination(int page, int pageSize) 
        {
            try
            {
                int skip = (page - 1) * pageSize;
                int totalCats = await dbContext.Cats.CountAsync();
                int totalPages = (int)Math.Ceiling(totalCats / (double)pageSize);

                List<CatEntity> cats =  await dbContext.Cats
                                                    .Include(c => c.CatTags)
                                                        .ThenInclude(ct => ct.TagEntity)
                                                    .Skip(skip)
                                                    .Take(pageSize)
                                                    .ToListAsync();

                GetCatsPegResponseVM obj = await CreateGetCatsPegResponseVMObject(cats, totalCats, page, pageSize, totalPages);
                return obj;
            }
            catch (Exception exc) 
            {
                //Some log
                return null;
            }
        }
        public async Task<GetCatsPegResponseVM> GetCatsWithPegination(string tag,int page, int pageSize)
        {
            try
            {
                int skip = (page - 1) * pageSize;
                int totalCats = await dbContext.Cats.Where(c => c.CatTags.Any(ct => ct.TagEntity.Name.Equals(tag))).CountAsync();
                int totalPages = (int)Math.Ceiling(totalCats / (double)pageSize);

                List<CatEntity> cats = await dbContext.Cats
                                                    .Include(c => c.CatTags)
                                                        .ThenInclude(ct => ct.TagEntity)
                                                    .Where(c => c.CatTags.Any(ct => ct.TagEntity.Name.Equals(tag)))
                                                    .Skip(skip)
                                                    .Take(pageSize)
                                                    .ToListAsync();

                GetCatsPegResponseVM obj = await CreateGetCatsPegResponseVMObject(cats, totalCats, page, pageSize, totalPages);
                return obj;
            }
            catch (Exception exc)
            {
                //Some log
                return null;
            }
        }
        private async Task<GetCatsPegResponseVM> CreateGetCatsPegResponseVMObject(List<CatEntity> cats,int totalCats,int page,int pageSize,int totalPages) 
        {
            GetCatsPegResponseVM resp = new GetCatsPegResponseVM
            {
                TotalCats = totalCats,
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = totalPages,
                Cats = new List<ReturnedCatsVM>()
            };

            foreach (CatEntity catWithTags in cats)
            {
                ReturnedCatsVM returnedCatsVM = new ReturnedCatsVM
                {
                    CatId = catWithTags.CatId,
                    Width = catWithTags.Width,
                    Height = catWithTags.Height,
                    Image = catWithTags.Image,
                    Tags = string.Join(',', catWithTags.CatTags.Select(ct => ct.TagEntity.Name).ToList())
                };
                resp.Cats.Add(returnedCatsVM);
            }
            return resp;
        }
        private async Task<List<CatVM>> GetCatsFromApi() 
        {
            try
            {
                List<CatVM> catsForReturn = new List<CatVM>();
                ExternalApiResponseVM responseVM = new ExternalApiResponseVM();
                responseVM = await externalApiService.GetDataFromExternalApiAsync();

                if (responseVM.IsSuccess)
                {
                    List<CatVM> cats = new List<CatVM>();
                    cats = JsonConvert.DeserializeObject<List<CatVM>>(responseVM.body);
                    return cats;
                }
                else
                {
                    return null;
                }

            }
            catch (JsonReaderException ex)
            {
                //Some Log
                return null;
            }
            catch (JsonSerializationException ex)
            {
                //Some Log
                return null;
            }
            catch (Exception ex) 
            {
                //Some Log
                return null;
            }
        }
        private bool InsertCatsToDbContext(List<CatVM> catVMs,ref HashSet<string> tags,out HashSet<string> insertedCats)
        {
            insertedCats = new HashSet<string>();
            try 
            {
                foreach (CatVM catVm in catVMs)
                {
                    if (!dbContext.Cats.Any(cat => cat.CatId.Equals(catVm.id)))
                    {
                        foreach (BreedVM breed in catVm.breeds) 
                        {
                            string[] curtags = breed.temperament.Split(',');
                            foreach (string t in curtags)
                            {
                                tags.Add(t.ToLower().Trim());
                            }
                        }
                        
                        CatEntity newCat = new CatEntity
                        {
                            CatId = catVm.id.Trim(),
                            Width = catVm.width,
                            Height = catVm.height,
                            Image = catVm.url.Trim(),
                            Created = DateTime.UtcNow
                        };
                        dbContext.Cats.Add(newCat);
                        insertedCats.Add(newCat.CatId);
                    }
                }
                return true;
            }
            catch (ArgumentException dbEx)
            {
                //Some Log
                return false;
            }
            catch (Exception ex)
            {
                //Some Log
                return false;
            }

        }
        private bool InsertTagsToDbContext(HashSet<string> tags)
        {
            try
            {
                foreach (string tag in tags)
                {
                    TagEntity existingTag = dbContext.Tags.FirstOrDefault(t => t.Name.Equals(tag.Trim()));
                    if (existingTag == null)
                    {
                        existingTag = new TagEntity
                        {
                            Name = tag.ToLower().Trim(),
                            Created = DateTime.UtcNow
                        };
                        dbContext.Tags.Add(existingTag);
                    }
                }
                return true;
            }
            catch (ArgumentException dbEx)
            {
                //Some Log
                return false;
            }
            catch (Exception ex)
            {
                //Some Log
                return false;
            }
        }
        private async Task<bool> InsertRelationsToDb(List<CatVM> catVMs)
        {
            using (var transaction = await dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    //Sava Cats and Tags to DB
                    int temp = dbContext.SaveChanges();

                    //Relationships Handling
                    foreach (var catVM in catVMs)
                    {

                        CatEntity existingCat = dbContext.Cats.FirstOrDefault(c => c.CatId.Equals(catVM.id.Trim()));
                        foreach (BreedVM breed in catVM.breeds)
                        {
                            string[] curtags = breed.temperament.Split(',');
                            foreach (string curtag in curtags)
                            {
                                TagEntity existingTag = dbContext.Tags.FirstOrDefault(te => te.Name.Equals(curtag.Trim()));
                                CatTag catTag = new CatTag
                                {
                                    CatEntityId = existingCat.Id,
                                    TagEntityId = existingTag.Id
                                };
                                dbContext.CatTags.Add(catTag);
                            }
                        }
                    }
                    //Sava Relationships to DB
                    temp = dbContext.SaveChanges();

                    await transaction.CommitAsync();
                    return true;
                }
                catch (DbUpdateException dbEx)
                {
                    //Some Log
                    return false;
                }
                catch (Exception ex)
                {
                    //Some Log
                    return false;
                }

            }
        }
    }
}
