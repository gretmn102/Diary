module CoreDiary
open System
open FsharpMyExtension

[<Measure>]
type Hour
[<Measure>]
type Minute
[<Measure>]
type Month
[<Measure>]
type Year
[<Measure>]
type Day

type Note = string

[<Serializable>]
type Hor = Map<int<Hour>*int<Minute>, Note>
[<Serializable>]
type Da = Map<int<Day>, Hor>
[<Serializable>]
type Mon = Map<int<Month>, Da>
[<Serializable>]
type DB = Map<int<Year>, Mon>
module Map =
    let mapSeq f (m:Map<_,_>) = m |> Seq.map(fun (KeyValue(k, v)) -> f k v)
    let filterSeq f (m:Map<_,_>) = m |> Seq.filter(fun (KeyValue(k, v)) -> f k v)
let map f (m:DB) =
    m
    |> Map.mapSeq (fun year ->
        Map.mapSeq(fun month ->
            Map.mapSeq (fun day ->
                Map.mapSeq (f year month day))
            >> Seq.concat)
        >> Seq.concat)
    |> Seq.concat
// let filter f (m:BD) =
//     m |> Map.filter (fun k -> Map.filter(fun k' x -> f k k' x))

type DateT = {
    Year:int<Year>
    Month:int<Month>
    Day:int<Day>
    Time:int<Hour> * int<Minute>
}
let ofDateTime (data:DateTime) =
    let time = data.TimeOfDay
    {
        Year = data.Year * 1<Year>
        Month = data.Month * 1<Month>
        Day = data.Day * 1<Day>
        Time = (time.Hours * 1<Hour>, time.Minutes * 1<Minute>)
    }

type BDSimple = Map<DateT, Note>

let upd year month day (m:DB) =
    let f k m =
        let v =
            match Map.tryFind k m with
            | None -> Map.empty
            | Some m' -> m'
        v, fun m' -> Map.add k m' m

    let v, m = f year m
    let v', m' = f month v
    let v'', m'' = f day v'

    (v'':Hor), ((m'' >> m' >> m) : Hor -> DB)

assert
    let m, f = upd 2017<Year> 1<Month> 20<Day> Map.empty
    let m = Map.add (20<Hour>, 30<Minute>) "some" <| Map.add (10<Hour>, 20<Minute>) "some happened in 10:20" m
    let _, f' = upd 2017<Year> 1<Month> 21<Day> <| f m
    f' m = ((Map.ofList
            [(2017<Year>,
              Map
                [(1<Month>,
                  Map
                    [(20<Day>,
                      Map [((10<Hour>, 20<Minute>), "some happened in 10:20"); ((20<Hour>, 30<Minute>), "some")]);
                     (21<Day>,
                      Map [((10<Hour>, 20<Minute>), "some happened in 10:20"); ((20<Hour>, 30<Minute>), "some")])])])]) : DB)

let addByDateTime (dateTime:System.DateTime) note (db:DB) =
    let hor, m = upd (dateTime.Year * 1<_>) (dateTime.Month * 1<_>) (dateTime.Day * 1<_>) db
    let hor = Map.add (dateTime.Hour * 1<_>, dateTime.Minute * 1<_>) note hor
    m hor
//module Ma =
//    open System
//    open System.Collections
//    open System.Collections.Generic
//    open Newtonsoft.Json
//
//    let mapConverter = {
//      new JsonConverter() with
//        override x.CanConvert(t:Type) =
//          t.IsGenericType && t.GetGenericTypeDefinition() = typedefof<Map<_, _>>
//
//        override x.WriteJson(writer, value, serializer) =
//          serializer.Serialize(writer, value)
//
//        override x.ReadJson(reader, t, _, serializer) =
//          let genArgs = t.GetGenericArguments()
//          let generify (t:Type) = t.MakeGenericType genArgs
//          let tupleType = generify typedefof<Tuple<_, _>>
//          let listType = typedefof<List<_>>.MakeGenericType tupleType
//          let create (t:Type) types = (t.GetConstructor types).Invoke
//          let list = create listType [||] [||] :?> IList
//          let kvpType = generify typedefof<KeyValuePair<_, _>>
//          for kvp in serializer.Deserialize(reader, generify typedefof<Dictionary<_, _>>) :?> IEnumerable do
//            let get name = (kvpType.GetProperty name).GetValue(kvp, null)
//            list.Add (create tupleType genArgs [|get "Key"; get "Value"|]) |> ignore
//          create (generify typedefof<Map<_, _>>) [|listType|] [|list|]
//    }
//    type mapConvert<'f,'t when 'f : comparison>() =
//        static member readJson (reader:JsonReader, serializer:JsonSerializer) =
//            serializer.Deserialize<Dictionary<'f, 't>> (reader)
//            |> Seq.map (fun kv -> kv.Key, kv.Value)
//            |> Map.ofSeq
//
//    let mapConverter = {
//      new JsonConverter() with
//        override __.CanConvert (t:Type) =
//          t.IsGenericType && t.GetGenericTypeDefinition() = typedefof<Map<_, _>>
//
//        override __.WriteJson (writer, value, serializer) =
//          serializer.Serialize(writer, value)
//
//        override __.ReadJson (reader, t, _, serializer) =
//          let converter =
//            typedefof<mapConvert<_,_>>.MakeGenericType (t.GetGenericArguments())
//
//          let readJson =
//            converter.GetMethod("readJson")
//
//          readJson.Invoke(null, [| reader; serializer |])
//    }
//    let str = JsonConvert.SerializeObject (Map<int*int, _> [(333,3), 1234])
//    JsonConvert.DeserializeObject<Map<(int*int), int>>(str, mapConverter)
//    mapConverter.CanConvert(Map[1,2] |> fun x -> x.GetType())

module Ser =
    let des (m:DB) =
        Map.toList m |> List.map (fun (k, v) ->
            k, Map.toList v |> List.map (fun (k, v) ->
                k, Map.toList v |> List.map (fun (k, v) -> k, Map.toList v )))
    let ser xs =
        xs |> List.map (fun (k, v) ->
            k, List.map (fun (k, v) ->
                k, List.map (fun (k, v) -> k, Map.ofList v) v |> Map.ofList) v
            |> Map.ofList)
        |> Map.ofList : DB
    let save path state = Json.serf path <| des state
    let load path = Json.desf path |> ser

//    assert
//#if INTERACTIVE
//        System.Environment.CurrentDirectory <- __SOURCE_DIRECTORY__
//#endif
//        let path = "Test.db"
//
//        let sa =
//            ((Map.ofList
//                    [(2017<Year>,
//                      Map
//                        [(1<Month>,
//                          Map
//                            [(20<Day>,
//                              Map [((10<Hour>, 20<Minute>), "some happened in 10:20"); ((20<Hour>, 30<Minute>), "some")]);
//                              (21<Day>, Map [((10<Hour>, 20<Minute>), "some happened in 10:20"); ((20<Hour>, 30<Minute>), "some")])
//                             ])])]) : BD)
//
//
//        let save st = Serialize.save path st
//
//        let saveLoad st = save st; Serialize.load path //: BD
//        //save (Map[(1<Hour>,2<Minute>), "str"]:TS)
//        let test (x:'a) = save x; (Serialize.load path : 'a) = x
//        let x = [Map[10, ""]; Map[10, ""];] |> List.map (Map.toList)
//        assert (test x)
//
//        assert
//            let x = des sa
//            save x;
//            let res = (Serialize.load path |> ser)
//            res = sa
//        true
type Date(path) =
    let mutable db = Ser.load path : DB

//    let add hour minute description (m:Hor) = Map.add (hour, minute) description m
//    member this.SaveDB() = ()
//    member this.Get year' month' day' =
//        let v, m = upd year' month' day' db
//        v, m
    let mutable m:Hor -> DB = fun _ -> db
    let mutable hor = Map.empty : Hor
    //member __.LoadDB() = db
    member __.GetDayNotes date =
        let date = ofDateTime date
        let hor', m' = upd date.Year date.Month date.Day db
        hor <- hor'; m <- m'
        hor |> Map.toArray
        |> Array.map (fun ((hour, min), note) ->
            sprintf "%A:%A" hour min, [| string hour; string min; note |])
    member __.AddToday date note =
        let date = ofDateTime date
        hor <- Map.add date.Time note hor
    member __.SaveHor() = db <- m hor
    member __.RemoveNote date =
        let date = ofDateTime date
        hor <- Map.remove date.Time hor
    member __.SaveDB() =
        Ser.save path db

let createDB path =
    let date = DateTime.Now |> ofDateTime
    let _, m = upd date.Year date.Month date.Day Map.empty
    let db = m (Map[date.Time, "create db"])
    Ser.save path db