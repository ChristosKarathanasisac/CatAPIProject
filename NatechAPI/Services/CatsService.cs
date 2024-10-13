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
        
        public CatsService(ApplicationDbContext context, ExternalApiService externalApiService, ConfigureServices configureServices)
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

        private async Task<List<CatVM>> GetCatsFromApi() 
        {
            try
            {
                List<CatVM> catsForReturn = new List<CatVM>();
                ResponseVM responseVM = new ResponseVM();
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
