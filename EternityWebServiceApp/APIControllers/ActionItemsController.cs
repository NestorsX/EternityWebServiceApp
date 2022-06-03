using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EternityWebServiceApp.Models;
using System.Linq;
using EternityWebServiceApp.ViewModels;

namespace EternityWebServiceApp.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActionItemsController : ControllerBase
    {
        private readonly EternityDBContext _context;

        public ActionItemsController(EternityDBContext context)
        {
            _context = context;
        }

        // Получает список закладок/закреплений/просмотров
        [HttpPost("all")]
        public async Task<IEnumerable<DataActionViewModel>> GetItemsByAction(DataAction data)
        {
            IEnumerable<DataActionViewModel> result = new List<DataActionViewModel>();
            IEnumerable<DataAction> dataActions = await _context.DataActions.Where(x => x.UserId == data.UserId && x.DataCategoryId == data.DataCategoryId && x.ActionCategoryId == data.ActionCategoryId).ToListAsync();
            foreach (var item in dataActions)
            {
                result = result.Append(new DataActionViewModel
                {
                    DataActionId = item.DataActionId,
                    UserId = item.UserId,
                    ItemId = (int)item.ItemId
                });
            }

            return result;
        }

        // Добавление закладки/закрепления/просмотра
        [HttpPost("add")]
        public async Task<ActionResult> PostCityBookmark(DataAction data)
        {
            if (await _context.DataActions.FirstOrDefaultAsync(x => x.UserId == data.UserId &&
                                                                    x.DataCategoryId == data.DataCategoryId &&
                                                                    x.ActionCategoryId == data.ActionCategoryId &&
                                                                    x.ItemId == data.ItemId) != null)
            {
                return Ok();
            }

            data.DataActionId = null;
            _context.DataActions.Add(data);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // Удаление закладки/закрепления/просмотра
        [HttpPost("remove")]
        public async Task<ActionResult> DeleteCityBookmark(DataAction data)
        {
            DataAction result = await _context.DataActions.FirstOrDefaultAsync(x => x.UserId == data.UserId &&
                                                                                    x.DataCategoryId == data.DataCategoryId &&
                                                                                    x.ActionCategoryId == data.ActionCategoryId &&
                                                                                    x.ItemId == data.ItemId);
            if (data == null)
            {
                return NotFound();
            }

            _context.DataActions.Remove(result);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
