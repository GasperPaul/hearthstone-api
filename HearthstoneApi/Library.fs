namespace HearthstoneApi
open FSharp.Data

module HearthstoneApi =

    type Locale = 
        | Locale of string

        static member enUS = Locale "enUS"
        static member enGB = Locale "enGB"
        static member deDE = Locale "deDE"
        static member esES = Locale "esES"
        static member esMX = Locale "esMX"
        static member frFR = Locale "frFR"
        static member itIT = Locale "itIT"
        static member koKR = Locale "koKR"
        static member plPL = Locale "plPL"
        static member ptBR = Locale "ptBR"
        static member ruRU = Locale "ruRU"
        static member zhCh = Locale "zhCH"
        static member zhTW = Locale "zhTW"
        static member jaJP = Locale "jaJP"
        static member thTH = Locale "thTH"

        override this.ToString() =
            let (Locale l) = this
            l
   

    type OptionalArgs(?attack: int, ?callback: string, ?collectible: int, ?cost: int, ?durability: int, ?health: int, ?locale: Locale) =
        let get name = Option.map (fun x -> (name, x.ToString()))

        member __.asList =
            [ 
                (get "attack" attack); 
                (get "callback" callback); 
                (get "collectible" collectible); 
                (get "cost" cost);
                (get "durability" durability); 
                (get "health" health); 
                (get "locale" locale) 
            ]
            |> List.choose id        


    type HearthstoneApi(api_token: string) =
        [<Literal>] 
        let API_ENTRY = "https://omgvamp-hearthstone-v1.p.mashape.com/"

        let call name (args:OptionalArgs option) = 
            try
                Http.RequestString ((sprintf "%s%s" API_ENTRY name), query = (match args with Some a -> a.asList | None -> []), headers = [ "X-Mashape-Key", api_token ])
                |> Ok
            with :? System.Net.WebException as ex -> 
                sprintf "Error while processing api request:\n%A"  ex
                |> Error


        member __.all_cards (?args) =
            call "cards" args

        member __.card_backs (?args) =
            call "cardbacks" args

        member __.card_search (name, ?args) =
            call (sprintf "cards/search/%s" name) args

        member __.card_set (set, ?args) =
            call (sprintf "cards/sets/%s" set) args

        member __.cards_by_class (``class``, ?args) = 
            call (sprintf "cards/classes/%s" ``class``) args

        member __.cards_by_faction (faction, ?args) = 
            call (sprintf "cards/factions/%s" faction) args

        member __.cards_by_quality (quality, ?args) = 
            call (sprintf "cards/qualities/%s" quality) args

        member __.cards_by_race (race, ?args) = 
            call (sprintf "cards/races/%s" race) args

        member __.cards_by_type (``type``, ?args) =
            call (sprintf "cards/types/%s" ``type``) args

        member __.info (?args) =
            call "info" args
        
        member __.single_card (name, ?args) =
            call (sprintf "cards/%s" name) args