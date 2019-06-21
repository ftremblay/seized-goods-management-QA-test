namespace SeizedGoodsManagement
{
    public interface IDatabase
    {
        int GetSolDNADelayForNatureCode(long natureCode);
        int GetSolDelayForNatureCode(long natureCode);
        int GetNonSolDelayForNatureCode(long natureCode);
        int GetNonSolExpertiseDelayForNatureCode(long natureCode);
    }
}