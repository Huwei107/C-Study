using SwiftCode.BBS.IServices;
using System;

namespace SwiftCode.BBS.Services
{
    public class CalculateService : ICalculateService
    {
        ICalculateService _calculateRepository = new CalculateService();
        public int Sum(int i,int j)
        {
            return _calculateRepository.Sum(i,j);
        }
    }
}
