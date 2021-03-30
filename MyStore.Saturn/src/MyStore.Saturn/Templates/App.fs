module MyStore.Saturn.Templates.App

open Giraffe.GiraffeViewEngine

open Global

let layout (content: XmlNode list) =
    html [ _class Bulma.HasNavbarFixedTop ] [
        head [] [
            meta [ _charset "utf-8" ]
            meta [ _name "viewport"
                   _content "width=device-width, initial-scale=1" ]
            title [] [ encodedText "MyStore" ]
            link [ _rel "stylesheet"
                   _href FontAwesomeSource ]
            link [ _rel "stylesheet"
                   _href BulmaSource ]
            link [ _rel "stylesheet"
                   _href "/app.css" ]
        ]
        body [] [
            yield
                nav [ classes [ Bulma.Navbar
                                Bulma.IsFixedTop
                                Bulma.HasShadow ] ] [
                    div [ _class Bulma.NavbarBrand ] [
                        a [ _class Bulma.NavbarItem
                            _href "https://github.com/prekel" ] [
                            img [ _src "https://avatars.githubusercontent.com/u/19646569?s=460"
                                  _width "28"
                                  _height "28" ]
                        ]
                    ]
                    div [ _class Bulma.NavbarMenu ] [
                        div [ _class Bulma.NavbarStart ] [
                            a [ _class Bulma.NavbarItem; _href "/" ] [
                                rawText "Главная страница"
                            ]
                            a [ _class Bulma.NavbarItem
                                _href "/products" ] [
                                rawText "Товары"
                            ]
                        ]
                    ]
                ]
            yield! content
            yield
                footer [ classes [ Bulma.Footer
                                   Bulma.IsFixedBottom ] ] [
                    div [ _class Bulma.Container ] [
                        div [ classes [ Bulma.Content
                                        Bulma.HasTextCentered ] ] [
                            p [] [
                                rawText "Vladislav Prekel - "
                                a [ _href "https://github.com/prekel/WebBackend" ] [
                                    rawText "MyStore"
                                ]
                                p [] [ str "2021" ]
                            ]
                        ]
                    ]
                ]
            yield script [ _src "/app.js" ] []
        ]
    ]
