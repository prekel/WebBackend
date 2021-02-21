module MyStore.Saturn.Templates.Index

open Giraffe.GiraffeViewEngine
open Global

let index =
    [ section [ classes [ Bulma.Hero; Bulma.IsPrimary ] ] [
        div [ _class Bulma.HeroBody ] [
            div [ _class Bulma.Container ] [
                div [ classes [ Bulma.Columns
                                Bulma.IsCentered ] ] [
                    div [ _class Bulma.Column ] [
                        p [ _class Bulma.Title ] [
                            rawText "MyStore"
                        ]
                    ]
                ]
            ]
        ]
      ]
      section [ _class Bulma.Section ] [
          div [ classes [ Bulma.Tile; Bulma.IsAncestor ] ] [
              div [ classes [ Bulma.Tile
                              Bulma.IsParent
                              Bulma.Is4 ] ] [
                  article [ classes [ Bulma.Tile
                                      Bulma.IsChild
                                      Bulma.Notification
                                      Bulma.IsLink
                                      Bulma.Box ] ] [
                      a [ _class Bulma.Title
                          _href "/products" ] [
                          rawText "Товары"
                      ]
                  ]
              ]
          ]
      ]
      //section [ _class Bulma.Section ] []
      //SignIn.signin
      ]

let layout = App.layout index
