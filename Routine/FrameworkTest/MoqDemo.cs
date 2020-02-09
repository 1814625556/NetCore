using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

namespace FrameworkTest
{
    [TestFixture]
    class MoqDemo
    {
        Mock<IFoo> mock;

        [SetUp]
        public void InitialSetup()
        {
            mock = new Mock<IFoo>();
        }

        [TearDown]
        public void TearDown()
        {
            mock = null;
        }

        [Test]
        public void Test1()
        {
            mock.Setup(foo => foo.DoSomething("ping")).Returns(true);
            //mock.SetupSet<string>( foo => foo.Name = "foo" );
            mock.Setup(f => f.Name).Returns("CHENCHANG");
            mock.Setup(foo => foo.GetCount()).Returns(100);

            mock.Object.GetCount();

            mock.Verify(foo => foo.GetCount(), Times.Once);

            mock.SetupAdd(m => m.MyEvent += It.IsAny<MyEventHandler>());
            mock.Raise(foo => foo.MyEvent += null, 25, true);

            var fol = mock.Object;
            var result = fol.GetCount();
            var flag = fol.DoSomething("pang");
            var name = fol.Name;
        }

        [Test]
        public void Test2()
        {
            var foo = new Mock<IFoo>();
            foo.Setup(f => f.DoSomething(1, "")).Returns(true);

            var flag = foo.Object.DoSomething(1, "");

            foo.Verify(f => f.DoSomething(1, ""), Times.Once);

            //foo.Verify(f=>f.DoSomething( 1,"" ),Times.Once);
            //foo.Verify();
        }

        [Test]
        public void TestProperty()
        {
            mock.Setup(foo => foo.Name).Returns("bar");

            //var name = mock.Object.Name;

            // auto-mocking hierarchies (a.k.a. recursive mocks)
            mock.Setup(foo => foo.Bar.Baz.Name).Returns("baz");

            //var barName = mock.Object.Bar.Baz.Name;

            // expects an invocation to set the value to "foo"
            mock.SetupSet(foo => foo.Name = "dog");

            mock.Object.Name = "dog";

            mock.SetupGet(foo => foo.Name);

            //name = mock.Object.Name;

            mock.VerifyGet(foo => foo.Name);
            // or verify the setter directly
            mock.VerifySet(foo => foo.Name = "dog");
        }

        public void CalcTrueOrFalse(int i, bool flag)
        {
            if (i < 10)
                flag = false;
            else
                flag = true;
        }

        public void CalcTrueOrFalse2(int i, bool flag)
        {
            flag = i > 100;
        }

        [Test]
        public void TestEvent()
        {
            /* 这两个是在一块使用的,这里依旧会报错，这是由于+=不是CalcTrueOrFalse,
               SetupAdd 就是验证 事件的 add remove操作的 即 +=，-=

            */
            //mock.SetupAdd(m => m.MyEvent += CalcTrueOrFalse2);
            //mock.VerifyAdd(m => m.MyEvent += CalcTrueOrFalse);

            mock.Raise(foo=>foo.MyEvent += CalcTrueOrFalse,1,true);
            mock.VerifyAdd(m => m.MyEvent += CalcTrueOrFalse);

            //这样子就可以raise了
            mock.Object.MyEvent += CalcTrueOrFalse;
            mock.Raise(foo => foo.MyEvent += null, 25, false);
        }

        public bool GetBoolean(int i)
        {
            return true;
        }

        [Test]
        public void TestEvent2()
        {
            //mock.Setup( foo => foo.DoSomething( "xx" ) );

            mock.Object.DoSomething("cc");
            mock.Verify(foo => foo.DoSomething("cc"));

            mock.Object.myboolEvent += GetBoolean;
            mock.Raise(foo => foo.myboolEvent += null, 2);
        }

        [Test]
        public void TestStrickMode()
        {
            var ccmock = new Mock<IFoo>(MockBehavior.Strict);
            ccmock.SetupAdd(foo => foo.MyEvent += null); //严格模式下这里必须是：MyEventHandler类型
            mock.Object.DoSomething("");
            //ccmock.Object.DoSomething( "" );//严格模式下，未被定义的行为被调用会报错

            mock.Object.MyEvent += null;
            ccmock.Object.MyEvent += CalcTrueOrFalse;
            ccmock.VerifyAdd(foo => foo.MyEvent += It.IsAny<MyEventHandler>());
        }

        [Test]
        public void TestException()
        {
            Assert.Throws<OutOfMemoryException>(() => throw new OutOfMemoryException());
        }

        [Test]
        public void TestVerify()
        {
            mock.Setup(foo => foo.DoSomething("ping")).Returns(true);

            //这个传参是必须的
            mock.Object.DoSomething("ping");
            mock.Verify(foo=>foo.DoSomething("ping"), "When doing operation X, the service should be pinged always");
        }
    

}
}
