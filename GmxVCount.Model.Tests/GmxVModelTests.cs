using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GmxVCount.Model.Tests
{
    [TestClass]
    public class GmxVModelTests
    {
        [TestMethod]
        public void TestSingleSave()
        {
            var gmx = new SingleResourceMetrics()
            {
                Stages =
                {
                    new Stage()
                    {
                        Phase = PhaseType.Initial,
                        Date = DateTime.Now,
                        SourceLanguage = "en-US",
                        TargetLanguage = "fr-FR",
                        CountGroups =
                        {
                            new CountGroup()
                            {
                                Name = VerifiableType.Verifiable,
                                Counts = new List<Count>()
                                {
                                    new Count()
                                    {
                                        CountType = CountType.TotalCharacterCount,
                                        Value = 30
                                    },
                                    new Count()
                                    {
                                        CountType = CountType.TotalWordCount,
                                        Value = 10
                                    }
                                }
                            }
                        }
                    },

                    new Stage()
                    {
                        Phase = PhaseType.Final,
                        Date = DateTime.Now,
                        SourceLanguage = "en-US",
                        TargetLanguage = "en-ES",
                        CountGroups =
                        {
                            new CountGroup()
                            {
                                Name = VerifiableType.Verifiable,
                                Counts = new List<Count>()
                                {
                                    new Count()
                                    {
                                        CountType = CountType.TotalCharacterCount,
                                        Value = 30
                                    },
                                    new Count()
                                    {
                                        CountType = CountType.TotalWordCount,
                                        Value = 10
                                    }
                                }
                            }
                        }
                    }
                }
            };

            gmx.Save(@"C:\Users\tflorin\Documents\stuff.gmx");
        }
    }
}
