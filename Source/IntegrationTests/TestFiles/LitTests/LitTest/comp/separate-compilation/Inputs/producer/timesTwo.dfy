module {:options "--function-syntax:4"} LibraryModule {

  function TimesTwo(x: nat): nat
  {
    x * 2
  }

  datatype Result<+T, +E> = Success(value: T) | Failure(error: E)

  // Record type
  datatype Pair<+T, +U> = Pair(first: T, second: U)

}
