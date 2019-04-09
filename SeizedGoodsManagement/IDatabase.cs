namespace SeizedGoodsManagement
{
    public interface IDatabase
    {
        float GetSolDNADelayForNatureCode(long natureCode);
        float GetSolDelayForNatureCode(long natureCode);
        float GetNonSolDelayForNatureCode(long natureCode);
        float GetNonSolExpertiseDelayForNatureCode(long natureCode);

        void Update(Item item);
    }
}