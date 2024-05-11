module Util

let (>.) x f = (fun y -> f y x)

let (./) x f = f x
let (/.) x y = x <| y
