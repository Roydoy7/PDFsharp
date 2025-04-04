﻿using MigraDoc.GrammarByExample;
using Xunit;

namespace GdiGrammarByExample
{
    /// <summary>
    /// Grammar by example unit test class.
    /// </summary>
    // ReSharper disable InconsistentNaming
    [Collection("GBE")]
    public class DdlGBE_Section_HeaderAndFooter : DdlGbeTestBase, IClassFixture<GbeFixture>
    {
        public DdlGBE_Section_HeaderAndFooter(GbeFixture fixture)
        {
            _fixture = fixture;
        }

        public override void TestInitialize()
        {
            InitializeTest(_fixture, "Section-HeaderAndFooter", 19, 0x7FFF0);
        }

        [Fact]
#if CORE
        public void DDL_Grammar_By_Example_Section_HeaderAndFooter()
#elif GDI
        public void GDI_DDL_Grammar_By_Example_Section_HeaderAndFooter()
#elif WPF
        public void WPF_DDL_Grammar_By_Example_Section_HeaderAndFooter()
#endif
        {
            RunTest();
        }
        // ReSharper restore InconsistentNaming

        readonly GbeFixture _fixture;
    }
}
