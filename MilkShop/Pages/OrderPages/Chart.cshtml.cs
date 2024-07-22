using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkShop.Business.OrderBusinesses;

namespace MilkShop.Pages.OrderPages
{
    public class ChartModel : PageModel
    {
        private readonly IOrderBusiness _orderBusiness;

        public ChartModel(IOrderBusiness orderBusiness)
        {
            _orderBusiness = orderBusiness;
        }

        [BindProperty(SupportsGet = true)]
        public DateTime FromDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime ToDate { get; set; }

        public List<int> OrderCounts { get; set; }
        public List<string> DateLabels { get; set; }

        public async Task OnGetAsync()
        {
           
            if (FromDate == default)
            {
                FromDate = DateTime.Now.AddDays(-6).Date;
                ToDate = DateTime.Now.Date;
            }

           
            if (FromDate > ToDate)
            {
                var temp = FromDate;
                FromDate = ToDate;
                ToDate = temp;
            }

            OrderCounts = new List<int>();
            DateLabels = new List<string>();

            for (DateTime date = FromDate; date <= ToDate; date = date.AddDays(1))
            {
                var result = await _orderBusiness.GetCountOrder(date, date);
                if (result.Status == 1) 
                {
                    OrderCounts.Add((int)result.Data);
                }
                else
                {
                    OrderCounts.Add(0);
                }
                DateLabels.Add(date.ToString("dd/MM"));
            }
        }
    }
}
