using NUnit.Framework;
using IntroSE.Kanban.Backend.BusinessLayer;
using System;
using Moq;

namespace Tests
{
    public class Tests
    {
        Board board;
        Mock<IColumn> columnA;
       
        [SetUp]
        public void Setup()
        {
            board = new Board("myTest@gmail.com", "Test");
            columnA = new Mock<IColumn>();
        }
        //all the function use DB so we create mock that agnore db
        /// <summary>
        /// Test on "AddBoard"
        /// </summary>
        /// <param name="Id"></param>
        [Test]
        [TestCase(4)][TestCase(100)][TestCase(-4)]
        public void Adding_Column_To_Place_That_Dosent_Exist_Fail(int Id)
        {
            try
            {
                board.AddColumnMock(Id, "needFail");
                Assert.Fail("should not add column to place that dosent exist");
            }
            catch (Exception)
            {
            }
        }
        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void Adding_ColumninId_Sucsess(int Id)
        {
            //act
            board.AddColumnMock(Id, "ok");
            //assert
            Assert.AreEqual(4, board.GetColumns().Count, "columns number need to be 4");
        }
        [Test]
        [TestCase("")]
        [TestCase("aaaa")]
        [TestCase("backlog")]
        [TestCase("done")]
        public void Adding_Column_Name_Sucsess(string Name)
        {
            //act
            board.AddColumnMock(0, Name);
            //assert
            Assert.AreEqual(4, board.GetColumns().Count, "columns number need to be 4");
        }

		[Test]
		[TestCase(0)]
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		public void Adding_Column_null_Fail(int Id)
		{
			try
			{
				board.AddColumnMock(Id, null);
				Assert.Fail("should not add null column");
			}
			catch (Exception)
			{
			}
		}
        /// <summary>
        /// Check "MoveColumn"- by mock
        /// </summary>
        [Test]
        public void MoveColumn_Sucsess()
        {
            //arrange
             Column columnB = board.GetColumn(0);
             columnA.Setup(m => m.isEmpty()).Returns(true);
            //act
            board.MoveColumnMock(0, 2);
			//assert
			Assert.AreEqual(columnB, board.GetColumn(2),"the column have to move 2 step");
		}
        [Test]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(100)]
        public void Move_Column_to_illegalPlace_Fail(int step)
        {
            try
            {
                columnA.Setup(m => m.isEmpty()).Returns(true);
                board.MoveColumnMock(0, step);
                Assert.Fail("should not move the column out of range");
            }
            catch (Exception)
            {
            }
        }
        [Test]
        public void Move_Column_dosent_Exist_Fail()
        {
            try
            {
                columnA.Setup(m => m.isEmpty()).Returns(true);
                board.MoveColumnMock(5, 2);
                Assert.Fail("should not move column that dosent exist");
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// check "RemoveColumn"
        /// </summary>
        [Test]
        public void RemoveColumn_Sucsess()
        {
            //act
            board.RemoveColumnMock(2);
            //assert
            Assert.AreEqual(2, board.GetColumns().Count, "columns number need to be 2");
        }
       
        [Test]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(100)]
        public void Remove_Column_dosent_Exist_Fail(int Id)
        {
            try
            {
                board.RemoveColumnMock(Id);
                Assert.Fail("should not remove column that dosent exist");
            }
            catch (Exception)
            {
            }
        }
        [Test]
        public void Remove_to_Many_Column_Fail()
        {
            try
            {
                //arrange
                board.RemoveColumnMock(0);
                //act
                board.RemoveColumnMock(0);
                //assert
                Assert.Fail("should not remove column because there is only 2 column left");
            }
            catch (Exception)
            {
            }
        }
    }
}