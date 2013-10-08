﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.IronLua;

namespace LuaDLR.Test
{
  public class TestParam
  {
    public int ParamOut(int a, ref int b, out int c)
    {
      c = b;
      b = a;
      return a;
    } // func ParamOut

    public int GetValue()
    {
      return Value;
    }

    public int Value { get { return 4; } }

    public int this[int i] { get { return i; } }
  } // class TestParam

  [TestClass]
  public class Functions : TestHelper
  {
    [TestMethod]
    public void TestFunctions01()
    {
      Assert.IsTrue(TestReturn("function test(a) return 1 + a; end; return test(2);", 3));
    } // proc TestFunctions01

    [TestMethod]
    public void TestFunctions02()
    {
      Assert.IsTrue(TestReturn("local function test(a) return 1 + a; end; return test(2);", 3));
    } // proc TestFunctions02

    [TestMethod]
    public void TestFunctions03()
    {
      Assert.IsTrue(TestReturn("local test = function (a) return 1 + a; end; return test(2);", 3));
    } // proc TestFunctions03

    [TestMethod]
    public void TestFunctions04()
    {
      Assert.IsTrue(TestReturn("local test = function () return 1, 2, 3; end; return (test());", 1));
    } // proc TestFunctions04

    [TestMethod]
    public void TestFunctions05()
    {
      Assert.IsTrue(TestReturn("local test = function () return 1, 2, 3; end; return test();", 1, 2, 3));
    } // proc TestFunctions05

    [TestMethod]
    public void TestFunctions06()
    {
      Assert.IsTrue(TestReturn("local test = function () return 1, 2, 3; end; return 'a', test();", "a", 1, 2, 3));
    } // proc TestFunctions06

    [TestMethod]
    public void TestFunctions07()
    {
      Assert.IsTrue(TestReturn("local test = function () return 3, 2, 1; end; return 2 * test();", 6));
    } // proc TestFunctions07

    [TestMethod]
    public void TestFunctions08()
    {
      Assert.IsTrue(TestReturn(GetCode("Lua.Function08.lua"), 1, 4));
    } // proc TestFunctions08

    [TestMethod]
    public void TestFunctions09()
    {
      Lua l = new Lua();
      l.PrintExpressionTree = true;
      object[] r = l.DoChunk("return p:ParamOut(1, 2);", "test.lua",
        new KeyValuePair<string, object>("p", new TestParam())
        );
      Assert.IsTrue(TestReturn(r, 1, 1, 2));
    } // proc TestFunctions09

    [TestMethod]
    public void TestFunctions10()
    {
      Lua l = new Lua();
      l.PrintExpressionTree = true;
      l["a"] = new TestParam();
      object[] r = l.DoChunk("local b = a:GetValue(); return b;", "test.lua");
      Assert.IsTrue(TestReturn(r, 4));
    } // proc TestFunctions10

    [TestMethod]
    public void TestFunctions11()
    {
      int[] test = new int[] { 1, 2, 3 };
      Lua l = new Lua();
      l.PrintExpressionTree = true;
      l["test"] = test;
      Assert.IsTrue(TestReturn(l.DoChunk("return test[0], test[1], test[2];", "test.lua"), 1, 2, 3));
    } // proc TestFunction11

    [TestMethod]
    public void TestFunctions12()
    {
      int[,] test = new int[,] { {1, 2}, {3, 4} };
      Lua l = new Lua();
      l.PrintExpressionTree = true;
      l["test"] = test;
      Assert.IsTrue(TestReturn(l.DoChunk("return test[0, 0], test[0, 1], test[1,0], test[1,1];", "test.lua"), 1, 2, 3, 4));
    } // proc TestFunction12

    [TestMethod]
    public void TestFunctions13()
    {
      Lua l = new Lua();
      l.PrintExpressionTree = true;
      l["test"] = new TestParam();
      Assert.IsTrue(TestReturn(l.DoChunk("return test[0], test[1];", "test.lua"), 0, 1));
    } // proc TestFunction13

    [TestMethod]
    public void TestFunctions14()
    {
      int[] test = new int[] { 1, 2, 3 };
      Lua l = new Lua();
      l.PrintExpressionTree = true;
      l["test"] = test;
      Assert.IsTrue(TestReturn(l.DoChunk("test[0] = 42; return test[0];", "test.lua"), 42));
    } // proc TestFunction14
  } // class Functions
}
