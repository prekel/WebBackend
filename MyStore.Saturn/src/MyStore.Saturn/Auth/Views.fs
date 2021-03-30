module MyStore.Saturn.Auth.Views

open Giraffe.GiraffeViewEngine

open MyStore.Saturn.Templates.Global
open MyStore.Saturn.Templates

let signIn =
    section [] [
        div [ _class Bulma.Field ] [
            p [ classes [ Bulma.Control
                          Bulma.HasIconsLeft
                          Bulma.HasIconsRight ] ] [
                input [ _class Bulma.Input
                        _type "email"
                        _placeholder "Email" ]
                span [ classes [ Bulma.Icon
                                 Bulma.IsSmall
                                 Bulma.IsLeft ] ] [
                    i [ classes [ FontAwesome.Fa
                                  FontAwesome.FaEnvelope ] ] []
                ]
                span [ classes [ Bulma.Icon
                                 Bulma.IsSmall
                                 Bulma.IsRight ] ] [
                    i [ classes [ FontAwesome.Fa
                                  FontAwesome.FaCheck ] ] []
                ]
            ]
        ]
        div [ _class Bulma.Field ] [
            p [ classes [ Bulma.Control
                          Bulma.HasIconsLeft ] ] [
                input [ _class Bulma.Input
                        _type "password"
                        _placeholder "Пароль" ]
                span [ classes [ Bulma.Icon
                                 Bulma.IsSmall
                                 Bulma.IsLeft ] ] [
                    i [ classes [ FontAwesome.Fa
                                  FontAwesome.FaLock ] ] []
                ]
            ]
        ]
        div [ _class Bulma.Field ] [
            p [ _class Bulma.Control ] [
                button [ classes [ Bulma.Button
                                   Bulma.IsSuccess ] ] [
                    str "Войти"
                ]
            ]
        ]
    ]

let layout = App.layout [ signIn ]
