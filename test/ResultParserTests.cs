using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using AspNetCore.HechinaSmsService;

namespace test
{
    public class ResultParserTests
    {
        [Theory]
        [InlineData("num=1&success=15902012345&faile=&err=国标二三一二&errid=0")]
        [InlineData("num=2&success=15902012345,15902012346&faile=&err=国标二三一二&errid=0")]
        [InlineData("num=3&success=15902012345,15902012346,15902012347&faile=&err=国标二三一二&errid=0")]
        public void Parse_Pass(string input)
        {
            var result = new HechinaGsendResponseMessage(input);
            Assert.Equal(0, result.ErrorCode);
        }

        [Theory]
        [InlineData("num=1&success=15902012345&faile=15902012346&err=国标二三一二&errid=-1")]
        [InlineData("num=1&success=&faile=15902012345&err=国标二三一二&errid=1")]
        [InlineData("num=1&success=&faile=15902012345&err=国标二三一二&errid=-1")]
        [InlineData("num=2&success=15902012345&faile=15902012346&err=国标二三一二&errid=1")]
        public void Parse_Fail(string input)
        {
            var result = new HechinaGsendResponseMessage(input);
            Assert.True(result.ErrorCode != 0);
        }

        [Fact]
        public void ParseOne_Pass()
        {
            var input = "num=1&success=15902012345&faile=&err=国标二三一二&errid=0";
            var result = new HechinaGsendResponseMessage(input);
            Assert.Equal(1, result.TotalCount);
            Assert.Equal(0, result.ErrorCode);
            Assert.Equal("15902012345", result.SucceededMobiles);
            Assert.Equal("", result.FailedMobiles);
        }
    }
}