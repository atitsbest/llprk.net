using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace llprk.Application.Test
{
    [TestClass]
    public class AutoMapperTest : TestBase
    {
        [TestMethod]
        public void Mapping_Configuration()
        {
            AutoMapper.Mapper.AssertConfigurationIsValid();
        }
    }
}
