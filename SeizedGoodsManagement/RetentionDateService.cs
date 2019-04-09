using System;

namespace SeizedGoodsManagement
{
    public class RetentionDateService
    {
        public DateTime ComputeRetentionDate(IDatabase database, Item item)
        {
            //The algorithm goes here

            //In every other cases, return item's creation date + 6 months
            return item.CreationDate.AddMonths(6);
        }
    }
}