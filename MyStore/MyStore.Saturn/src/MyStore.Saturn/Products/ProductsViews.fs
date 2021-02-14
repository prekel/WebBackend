namespace Products

open Microsoft.AspNetCore.Http
open Giraffe.GiraffeViewEngine
open Saturn



module Views =

    let product product =
        div [ _class "card" ] [
            //            div [ _class "card-image" ] [
//                figure [ _class "image is-4by3" ] [
//                    img [ _src "https://bulma.io/images/placeholders/1280x960.png"
//                          _alt "qwerty" ]
//                ]
//            ]
            div [ _class "card-content" ] [
                str "qwerty1"
                str product.Name
            ]
        ]

    let index (ctx: HttpContext) (objs: Product list) =
        let cnt =
            [ div [ _class "container " ] [
                h2 [ _class "title" ] [
                    encodedText "Listing Products"
                ]
                div [] [
                    for o in objs |> List.splitInto 3 do
                        yield div [ _class "columns" ] [
                            div [_class "column"] []
                        ]
                        yield div [] [ product (o|> List.head) ] 
                ]
                a [ _class "button is-text"
                    _href (Links.add ctx) ] [
                    encodedText "New Product"
                ]
              ] ]

        App.layout ([ section [ _class "section" ] cnt ])


    let show (ctx: HttpContext) (o: Product) =
        let cnt =
            [ div [ _class "container " ] [
                h2 [ _class "title" ] [
                    encodedText "Show Product"
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
                ]
                a [ _class "button is-text"
                    _href (Links.edit ctx o.ProductId) ] [
                    encodedText "Edit"
                ]
                a [ _class "button is-text"
                    _href (Links.index ctx) ] [
                    encodedText "Back"
                ]
              ] ]

        App.layout ([ section [ _class "section" ] cnt ])

    let private form (ctx: HttpContext) (o: Product option) (validationResult: Map<string, string>) isUpdate =
        let validationMessage =
            div [ _class "notification is-danger" ] [
                a [ _class "delete"
                    attr "aria-label" "delete" ] []
                encodedText "Oops, something went wrong! Please check the errors below."
            ]

        let field selector lbl key =
            div [ _class "field" ] [
                yield
                    label [ _class "label" ] [
                        encodedText (string lbl)
                    ]
                yield
                    div [ _class "control has-icons-right" ] [
                        yield
                            input [ _class (if validationResult.ContainsKey key then "input is-danger" else "input")
                                    _value (defaultArg (o |> Option.map selector) "")
                                    _name key
                                    _type "text" ]
                        if validationResult.ContainsKey key then
                            yield
                                span [ _class "icon is-small is-right" ] [
                                    i [ _class "fas fa-exclamation-triangle" ] []
                                ]
                    ]
                if validationResult.ContainsKey key then
                    yield
                        p [ _class "help is-danger" ] [
                            encodedText validationResult.[key]
                        ]
            ]

        let buttons =
            div [ _class "field is-grouped" ] [
                div [ _class "control" ] [
                    input [ _type "submit"
                            _class "button is-link"
                            _value "Submit" ]
                ]
                div [ _class "control" ] [
                    a [ _class "button is-text"
                        _href (Links.index ctx) ] [
                        encodedText "Cancel"
                    ]
                ]
            ]

        let cnt =
            [ div [ _class "container " ] [
                form [ _action (if isUpdate then Links.withId ctx o.Value.ProductId else Links.index ctx)
                       _method "post" ] [
                    if not validationResult.IsEmpty then yield validationMessage
                    yield field (fun i -> (string i.ProductId)) "ProductId" "ProductId"
                    yield field (fun i -> (string i.Name)) "Name" "Name"
                    yield field (fun i -> (string i.Description)) "Description" "Description"
                    yield field (fun i -> (string i.Price)) "Price" "Price"
                    yield buttons
                ]
              ] ]

        App.layout ([ section [ _class "section" ] cnt ])

    let add (ctx: HttpContext) (o: Product option) (validationResult: Map<string, string>) =
        form ctx o validationResult false

    let edit (ctx: HttpContext) (o: Product) (validationResult: Map<string, string>) =
        form ctx (Some o) validationResult true
