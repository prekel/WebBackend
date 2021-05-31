namespace Fable.System.ComponentModel.Annotations

open System


[<AttributeUsage(AttributeTargets.Property
                 ||| AttributeTargets.Field
                 ||| AttributeTargets.Parameter,
                 AllowMultiple = false)>]
type RequiredAttribute() =
    inherit Attribute()
