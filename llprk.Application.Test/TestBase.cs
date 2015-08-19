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
    public class TestBase
    {
        /// <summary>
        /// CTR
        /// </summary>
        public TestBase()
        {
            AutoMapper.Mapper.AddProfile<Llprk.Application.AutoMapperProfile>();
        }

        /// <summary>
        /// hier kann in einer Transaction (ohne commit) in der Datenbank gearbeitet werden.
        /// Wirft eine Exception wenn, das angegebene IQueryable sich nicht um <paramref name="amount"/> geändert hat.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="fn"></param>
        /// <param name="amount"></param>
        protected void AssertChangedBy<T>(IQueryable<T> db, Action fn, int amount = 1)
        {
            using (var tx = new TransactionScope())
            {
                var count = db.Count();

                fn();

                Assert.AreEqual(count + amount, db.Count());
            }
        }
    }
}
