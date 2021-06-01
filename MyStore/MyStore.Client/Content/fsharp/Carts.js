import { Record, Union } from "./.fable/fable-library.3.1.11/Types.js";
import { obj_type, record_type, array_type, union_type, class_type, string_type, bool_type, int32_type } from "./.fable/fable-library.3.1.11/Reflection.js";
import { CartsModel$reflection } from "./MyStore.Dto/Shop.js";
import { Cart_ToDomain_268DEFC0, Cart$reflection } from "./MyStore.Domain/Shop.js";
import { map } from "./.fable/fable-library.3.1.11/Array.js";
import { extra, acceptJson, baseUrl, pageToOffset, itemsPerPage, offsetToPage } from "./Extensions.js";
import { Cmd_ofSub, Cmd_OfPromise_either, Cmd_none } from "./.fable/Fable.Elmish.3.1.0/cmd.fs.js";
import { PromiseBuilder__Delay_62FBFDE1, PromiseBuilder__Run_212F1D4B } from "./.fable/Fable.Promise.2.0.0/Promise.fs.js";
import { interpolate, toText } from "./.fable/fable-library.3.1.11/String.js";
import { promise } from "./.fable/Fable.Promise.2.0.0/PromiseImpl.fs.js";
import { Fetch_get_5760677E } from "./.fable/Thoth.Fetch.2.0.0/Fetch.fs.js";
import { CaseStrategy } from "./.fable/Thoth.Json.4.0.0/Types.fs.js";
import { uncurry } from "./.fable/fable-library.3.1.11/Util.js";
import { RouterModule_nav } from "./.fable/Feliz.Router.3.8.0/Router.fs.js";
import { singleton } from "./.fable/fable-library.3.1.11/List.js";
import { some } from "./.fable/fable-library.3.1.11/Option.js";
import { useFeliz_React__React_useElmish_Static_645B1FB7 } from "./.fable/Feliz.UseElmish.1.5.1/UseElmish.fs.js";
import { map as map_1, singleton as singleton_1, append, delay, toList } from "./.fable/fable-library.3.1.11/Seq.js";
import { createElement } from "react";
import { Interop_reactApi } from "./.fable/Feliz.1.45.0/Interop.fs.js";

export class Msg extends Union {
    constructor(tag, ...fields) {
        super();
        this.tag = (tag | 0);
        this.fields = fields;
    }
    cases() {
        return ["SetPage", "SetPublic", "Fetched", "Failed"];
    }
}

export function Msg$reflection() {
    return union_type("MyStore.Client.Carts.Msg", [], Msg, () => [[["Item", int32_type]], [["Item", bool_type]], [["Item1", CartsModel$reflection()], ["Item2", string_type]], [["Item", class_type("System.Exception")]]]);
}

export class State extends Record {
    constructor(Carts, Page, IsPublic) {
        super();
        this.Carts = Carts;
        this.Page = (Page | 0);
        this.IsPublic = IsPublic;
    }
}

export function State$reflection() {
    return record_type("MyStore.Client.Carts.State", [], State, () => [["Carts", array_type(Cart$reflection())], ["Page", int32_type], ["IsPublic", bool_type]]);
}

export function init(cartsModel, unitVar1) {
    return [new State(map((arg00) => Cart_ToDomain_268DEFC0(arg00), cartsModel.carts), offsetToPage(cartsModel.query.offset), cartsModel.query.isPublic), Cmd_none()];
}

export function getCarts(isPublic, page) {
    return PromiseBuilder__Run_212F1D4B(promise, PromiseBuilder__Delay_62FBFDE1(promise, () => {
        const url = toText(interpolate("/Shop/Cart?isPublic=%b%P()\u0026count=%i%P()\u0026offset=%i%P()", [isPublic, itemsPerPage, pageToOffset(page)]));
        return Fetch_get_5760677E(baseUrl() + url, void 0, void 0, acceptJson, new CaseStrategy(1), extra, uncurry(2, void 0), {
            ResolveType: CartsModel$reflection,
        }, {
            ResolveType: () => obj_type,
        }).then(((_arg1) => (Promise.resolve((new Msg(2, _arg1, url))))));
    }));
}

export function update(msg, state) {
    switch (msg.tag) {
        case 1: {
            const isPublic_1 = msg.fields[0];
            return [new State(state.Carts, state.Page, isPublic_1), Cmd_OfPromise_either((tupledArg_1) => getCarts(tupledArg_1[0], tupledArg_1[1]), [isPublic_1, state.Page], (x_1) => x_1, (arg0_1) => (new Msg(3, arg0_1)))];
        }
        case 2: {
            return [new State(map((arg00) => Cart_ToDomain_268DEFC0(arg00), msg.fields[0].carts), state.Page, state.IsPublic), Cmd_ofSub((_arg138) => {
                RouterModule_nav(singleton(msg.fields[1]), 1, 2);
            })];
        }
        case 3: {
            console.error(some(msg.fields[0]));
            return [state, Cmd_none()];
        }
        default: {
            const page = msg.fields[0] | 0;
            return [new State(state.Carts, page, state.IsPublic), Cmd_OfPromise_either((tupledArg) => getCarts(tupledArg[0], tupledArg[1]), [state.IsPublic, page], (x) => x, (arg0) => (new Msg(3, arg0)))];
        }
    }
}

export function Carts(carts) {
    const patternInput = useFeliz_React__React_useElmish_Static_645B1FB7(() => init(carts, void 0), (msg, state) => update(msg, state), []);
    const state_1 = patternInput[0];
    const dispatch = patternInput[1];
    const children = toList(delay(() => append(singleton_1(createElement("input", {
        checked: state_1.IsPublic,
        type: "checkbox",
        onChange: (ev) => {
            dispatch(new Msg(1, ev.target.checked));
        },
    })), delay(() => append(singleton_1(createElement("button", {
        children: "+",
        onClick: (_arg1) => {
            dispatch(new Msg(0, state_1.Page + 1));
        },
    })), delay(() => {
        let value_8;
        return append(singleton_1((value_8 = toText(interpolate("Page: %i%P()", [state_1.Page])), createElement("p", {
            children: [value_8],
        }))), delay(() => append(singleton_1(createElement("button", {
            children: "-",
            onClick: (_arg2) => {
                dispatch(new Msg(0, state_1.Page - 1));
            },
        })), delay(() => map_1((i) => {
            let value_12;
            return createElement("a", {
                children: Interop_reactApi.Children.toArray([(value_12 = toText(interpolate("%A%P()", [i])), createElement("p", {
                    children: [value_12],
                }))]),
                href: toText(interpolate("/Shop/Cart/%i%P()", [i.CartId])),
            });
        }, state_1.Carts)))));
    }))))));
    return createElement("div", {
        children: Interop_reactApi.Children.toArray(Array.from(children)),
    });
}

