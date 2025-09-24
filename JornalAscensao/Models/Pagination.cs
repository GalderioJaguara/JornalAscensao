using Microsoft.EntityFrameworkCore;

namespace JornalAscensao.Models;

public class Pagination<T>
{
   public List<T> Items { get; set; }
   public int PageSize { get; set; }
   public int PageIndex { get; set; }
   public int TotalPages { get; set; }
   public int TotalItems { get; set; }

   public bool HasPreviousPage => PageIndex > 1;
   public bool HasNextPage => PageIndex < TotalPages;
   public int StartIndex => Math.Max(1, PageIndex - 2);
   public int EndIndex => Math.Min(TotalPages, PageIndex + 2);
   public bool ShowStartEllipsis => StartIndex > 1;
   public bool ShowEndEllipsis => EndIndex < TotalPages;



   public Pagination(List<T> items, int pageSize, int pageIndex, int totalItems)
   {
      Items = items;
      PageSize = pageSize;
      PageIndex = pageIndex;
      TotalItems = totalItems;
      TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
   }

   public static async Task<Pagination<T>> GetItemsPaginados(IQueryable<T> query, int pageIndex, int pageSize)
   {
      if (query == null)
         throw new ArgumentNullException(nameof(query));
      if (pageIndex < 1)
         pageIndex = 1;
      if (pageSize > 50)
         pageSize = 50;

      var totalItems = await query.CountAsync();

      if (totalItems == 0)
         return new Pagination<T>(new List<T>(), pageSize, pageIndex, 0);

      var maxPages = (int)Math.Ceiling((double)totalItems / pageSize);
      if (pageIndex > maxPages)
         pageIndex = maxPages;

      var items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
      return new Pagination<T>(items, pageSize, pageIndex, totalItems);
   }

   public IEnumerable<int> GetPageRange()
   {
      return Enumerable.Range(StartIndex, EndIndex - StartIndex + 1);
   }

}