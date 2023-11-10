module DynamicArrayExamples {
  import opened DafnyStdLibs.DynamicArray
  import opened DafnyStdLibs.BoundedInts

  method {:test} Ensure() {
    var arr := new DynamicArray<int>();
    arr.Ensure(101, 3);
    assert arr.capacity >= 101;
    assert arr.size == 0;
    for i: int := 0 to 100
      invariant fresh(arr.Repr)
      invariant arr.Valid?()
      invariant arr.size as int == i
      invariant arr.capacity >= 101
    {
      arr.PushFast(i);
    }
  }

  method {:test} Push_At_Put_PopFast_PushFast() {
    var arr := new DynamicArray<int>();
    for i: int := 0 to 1000
      invariant arr.Valid?()
      invariant fresh(arr.Repr)
      invariant arr.size as int == i
    {
      arr.Push(i);
    }

    expect arr.At(30) == 30;
    arr.Put(30, 31);
    expect arr.At(30) == 31;

    arr.PopFast();
    arr.PopFast();
    arr.PushFast(3);
    arr.PushFast(4);
  }
}