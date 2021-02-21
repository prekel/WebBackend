module MyStore.Saturn.Products.Views

open Microsoft.AspNetCore.Http
open Giraffe.GiraffeViewEngine
open Saturn

open MyStore.Saturn.Templates.Global
open MyStore.Saturn.Templates

let rating r =
    let arr1 = List.init r (fun _ -> true)

    let arr2 = List.init (10 - r) (fun _ -> false)

    let arr = arr1 @ arr2

    arr
    |> List.chunkBySize 2
    |> List.map (function
        | [ true; true ] -> FontAwesome.FaStar
        | [ true; false ] -> FontAwesome.FaStarHalfEmpty
        | [ false; false ] -> FontAwesome.FaStarO
        | _ -> "")
    |> List.map (fun cl -> div [ classes [ FontAwesome.Fa; cl ] ] [])

let product (ctx: HttpContext) product =
    div [ _class Bulma.Card ] [
        div [ _class Bulma.CardImage ] [
            figure [ classes [ Bulma.Image; Bulma.Is4By3 ] ] [
                img [ _src "https://bulma.io/images/placeholders/1280x960.png"
                      _alt "qwerty" ]
            ]
        ]
        div [ _class Bulma.CardContent ] [
            div [ _class Bulma.Columns ] [
                a [ _class Bulma.Column
                    _href (Links.withId ctx product.ProductId) ] [
                    str product.Name
                ]
                div [ _class Bulma.Column ] [
                    str <| string product.Price
                ]
            ]
            div [] [ str product.Description ]
            div [ _class Bulma.Columns ] [
                div [ _class Bulma.Column ] [
                    yield! rating product.Rating
                ]
                div [ _class Bulma.Column ] []
            ]
        ]
    ]

let index (ctx: HttpContext) (objs: Product list) =
    let cnt =
        [ div [ _class Bulma.Container ] [
            h2 [ _class Bulma.Title ] [
                str "Товары"
            ]
            div [] [
                for o in objs |> List.chunkBySize 3 do
                    yield
                        div [ _class Bulma.Columns ] [
                            for p in o do
                                yield
                                    div [ classes [ Bulma.Column ] ] [
                                        product ctx p
                                    ]
                            yield!
                                [ o |> List.length .. 3 - 1 ]
                                |> List.map (fun _ -> div [ _class Bulma.Column ] [])
                        ]
            ]
            a [ classes [ Bulma.Button; Bulma.IsText ]
                _href <| Links.add ctx ] [
                encodedText "Добавить товар"
            ]
          ] ]

    App.layout ([ section [ _class Bulma.Section ] cnt ])


let show (ctx: HttpContext) (o: Product) =
    let cnt =
        [ div [ _class Bulma.Container ] [
            h2 [ _class Bulma.Title ] [
                str "Товар"
            ]
            ul [] [
                li [] [
                    strong [] [ encodedText "ProductId: " ]
                    encodedText (string o.ProductId)
                ]
                li [] [
                    strong [] [ encodedText "Name: " ]
                    encodedText (string o.Name)
                ]
                li [] [
                    strong [] [
                        encodedText "Description: "
                    ]
                    encodedText (string o.Description)
                ]
                li [] [
                    strong [] [ encodedText "Price: " ]
                    encodedText (string o.Price)
                ]
                li [] [
                    strong [] [ encodedText "Rating: " ]
                    yield! rating o.Rating
                ]
            ]
            a [ classes [ Bulma.Button; Bulma.IsText ]
                _href (Links.edit ctx o.ProductId) ] [
                encodedText "Изменить"
            ]
            a [ classes [ Bulma.Button; Bulma.IsText ]
                _href (Links.index ctx) ] [
                encodedText "Назад"
            ]
          ] ]

    App.layout ([ section [ _class Bulma.Section ] cnt ])

let private form (ctx: HttpContext) (o: Product option) (validationResult: Map<string, string>) isUpdate =
    let validationMessage =
        div [ classes [ Bulma.Notification
                        Bulma.IsDanger ] ] [
            a [ _class Bulma.Delete
                attr "aria-label" "delete" ] []
            str "Oops, something went wrong! Please check the errors below."
        ]

    let field selector lbl key =
        div [ _class Bulma.Field ] [
            yield
                label [ _class Bulma.Label ] [
                    encodedText (string lbl)
                ]
            yield
                div [ classes [ Bulma.Control
                                Bulma.HasIconsRight ] ] [
                    yield
                        input [ classes
                                    (if validationResult.ContainsKey key then
                                        [ Bulma.Input; Bulma.IsDanger ]
                                     else
                                         [ Bulma.Input ])
                                _value (defaultArg (o |> Option.map selector) "")
                                _name key
                                _type "text" ]
                    if validationResult.ContainsKey key then
                        yield
                            span [ classes [ Bulma.Icon
                                             Bulma.IsSmall
                                             Bulma.IsRight ] ] [
                                i [ classes [ FontAwesome.Fa
                                              FontAwesome.FaExclamationTriangle ] ] []
                            ]
                ]
            if validationResult.ContainsKey key then
                yield
                    p [ classes [ Bulma.Help; Bulma.IsDanger ] ] [
                        encodedText validationResult.[key]
                    ]
        ]

    let buttons =
        div [ classes [ Bulma.Field; Bulma.IsGrouped ] ] [
            div [ _class Bulma.Control ] [
                input [ _type "submit"
                        classes [ Bulma.Button; Bulma.IsLink ]
                        _value "Отправить" ]
            ]
            div [ _class Bulma.Control ] [
                a [ classes [ Bulma.Button; Bulma.IsText ]
                    _href (Links.index ctx) ] [
                    encodedText "Отмена"
                ]
            ]
        ]

    let cnt =
        [ div [ _class Bulma.Container ] [
            form [ _action (if isUpdate then Links.withId ctx o.Value.ProductId else Links.index ctx)
                   _method "post" ] [
                if not validationResult.IsEmpty then yield validationMessage
                yield field (fun i -> (string i.Name)) "Name" "Name"
                yield field (fun i -> (string i.Description)) "Description" "Description"
                yield field (fun i -> (string i.Price)) "Price" "Price"
                yield field (fun i -> (string i.Rating)) "Rating" "Rating"
                yield buttons
            ]
          ] ]

    App.layout ([ section [ _class Bulma.Section ] cnt ])

let add (ctx: HttpContext) (o: Product option) (validationResult: Map<string, string>) =
    form ctx o validationResult false

let edit (ctx: HttpContext) (o: Product) (validationResult: Map<string, string>) =
    form ctx (Some o) validationResult true
