module MyStore.Saturn.Templates.Global

open Giraffe.GiraffeViewEngine
open Zanaptak.TypedCssClasses

[<Literal>]
let BulmaSource =
    "https://cdnjs.cloudflare.com/ajax/libs/bulma/0.6.1/css/bulma.min.css"

type Bulma = CssClasses<BulmaSource, Naming.PascalCase>

[<Literal>]
let FontAwesomeSource =
    "https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css"

type FontAwesome = CssClasses<FontAwesomeSource, Naming.PascalCase>

let classes c = c |> String.concat " " |> _class
