using System;

namespace SeizedGoodsManagement
{
    public class Item
    {
        public bool IsDNAPositive { get; set; }
        public bool IsExpertisePositive { get; set; }
        public DispositionCode DispositionCode { get; set; }
        public NatureCode NatureCode { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
