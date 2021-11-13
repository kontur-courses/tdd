using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class SortedDistinctSingleLinkedListTests
    {
        [Test]
        public void SortedDistinctSingleLinkedList_ThrowArgumentException_NullComparator()
        {
            Action act = () => new SortedDistinctSingleLinkedList<int>(null);
            act.Should().Throw<ArgumentException>()
                .WithMessage("Comparator mustn't be null");
        }
        
        [Test]
        public void ToEnumerable_ListsAllItems()
        {
            var list = new SortedDistinctSingleLinkedList<int>((a, b) => a < b);
            var itemsToAdd = Enumerable.Range(0, 10).ToArray();
            foreach (var element in itemsToAdd)
                list.Add(element);
            list.ToEnumerable().Should().HaveCount(10).And.Contain(itemsToAdd);
        }

        [Test]
        public void Add_ItemShouldBeSavedAfterFirstAdd()
        {
            var list = new SortedDistinctSingleLinkedList<int>((a, b) => a < b);
            list.Add(1);
            list.ToEnumerable().Should().ContainSingle().And.Contain(1);
        }

        [TestCase(1)]
        [TestCase(54)]
        public void Add_ItemsShouldBeSavedAfterAdd(int count)
        {
            var list = new SortedDistinctSingleLinkedList<int>((a, b) => a < b);
            var itemsToAdd = Enumerable.Range(0, count).ToList();
            var itemToAdd = count + 1;
            foreach (var item in itemsToAdd)
                list.Add(item);
            list.Add(itemToAdd);
            list.ToEnumerable().Should().HaveCount(count + 1)
                .And.Contain(itemsToAdd)
                .And.Contain(itemsToAdd);
        }
        
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(10)]
        public void Add_ListShouldBeSortedAfterAdd(int count)
        {
            var list = new SortedDistinctSingleLinkedList<int>((a, b) => a < b);
            var itemsToAdd = Enumerable.Range(0, count).Reverse();
            foreach (var item in itemsToAdd)
                list.Add(item);
            list.ToEnumerable().Should().BeInAscendingOrder();
        }

        [TestCase(1, 5)]
        [TestCase(2, 5)]
        [TestCase(10, 5)]
        public void Add_ItemsMustBeUniqueAfterAdd(int uniqueItemsCount, int repeatedItemsCount)
        {
            var list = new SortedDistinctSingleLinkedList<int>((a, b) => a < b);
            var itemsToAdd = Enumerable.Range(0, repeatedItemsCount * uniqueItemsCount)
                .Select(x => x % uniqueItemsCount);
            foreach (var item in itemsToAdd)
                list.Add(item);
            list.ToEnumerable().Should().HaveCount(uniqueItemsCount)
                .And.Contain(Enumerable.Range(0, uniqueItemsCount));
        }
    }
}