using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Teashop.Backend.Application.Commons.Models;
using Xunit;

namespace Teashop.Backend.Tests.UnitTests.Application.Common.Models
{
    public class PaginatedListTests
    {
        [Fact]
        public void WhenItemsOnInputOnlyThenCreateWithCorrectValues()
        {
            var paginatedList = new PaginatedList<int>(GetList(10));

            paginatedList.PageIndex.Should().Be(0);
            paginatedList.PageSize.Should().Be(10);
            paginatedList.PagesInTotal.Should().Be(1);
            paginatedList.TotalCount.Should().Be(10);
        }

        [Fact]
        public void WhenPageSizeIsLargerTotalCountThenCreateWithPageSizeEqualToTotalCount()
        {
            var paginatedList = new PaginatedList<int>(GetList(5), 0, 10, 5);

            paginatedList.PageIndex.Should().Be(0);
            paginatedList.PageSize.Should().Be(5);
            paginatedList.PagesInTotal.Should().Be(1);
            paginatedList.TotalCount.Should().Be(5);
        }

        [Fact]
        public void WhenPagesInTotalDivisionHasRemainderThenCreateWithPagesInTotalCalculatedCorrectly()
        {
            var paginatedList = new PaginatedList<int>(GetList(3), 2, 3, 10);

            paginatedList.PageIndex.Should().Be(2);
            paginatedList.PageSize.Should().Be(3);
            paginatedList.PagesInTotal.Should().Be(4);
            paginatedList.TotalCount.Should().Be(10);
        }

        private List<int> GetList(int numberOfElements)
        {
            return Enumerable.Repeat(1, numberOfElements)
                .ToList();
        }
    }
}
