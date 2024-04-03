// RUN: %verify --unicode-char false "%s" > "%t"
// RUN: ! %run --no-verify --unicode-char false --target cs "%s" >> "%t"
// RUN: ! %run --no-verify --unicode-char false --target go "%s" >> "%t"
// RUN: ! %run --no-verify --unicode-char false --target java "%s" >> "%t"
// RUN: ! %run --no-verify --unicode-char false --target js "%s" >> "%t"
// RUN: ! %run --no-verify --unicode-char false --target py "%s" >> "%t"
// RUN: %diff "%s.expect" "%t"

datatype Option<T> = None | Some(get: T)

method Main() {
  var x := Some("where over the rainbow");
  expect x.None?, x;
}
