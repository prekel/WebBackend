import { int, object } from "./.fable/Thoth.Json.4.0.0/Decode.fs.js";
import { some, ofNullable, toNullable } from "./.fable/fable-library.3.1.11/Option.js";
import { uncurry } from "./.fable/fable-library.3.1.11/Util.js";
import { option as option_1 } from "./.fable/Thoth.Json.4.0.0/Encode.fs.js";
import { newGuid } from "./.fable/fable-library.3.1.11/Guid.js";
import { add } from "./.fable/fable-library.3.1.11/Map.js";
import { empty } from "./.fable/Thoth.Json.4.0.0/Extra.fs.js";
import { CaseStrategy, ExtraCoders } from "./.fable/Thoth.Json.4.0.0/Types.fs.js";
import { awaitPromise } from "./.fable/fable-library.3.1.11/Async.js";
import { PromiseBuilder__Delay_62FBFDE1, PromiseBuilder__Run_212F1D4B } from "./.fable/Fable.Promise.2.0.0/Promise.fs.js";
import { printf, interpolate, toText } from "./.fable/fable-library.3.1.11/String.js";
import { promise } from "./.fable/Fable.Promise.2.0.0/PromiseImpl.fs.js";
import { Fetch_get_5760677E } from "./.fable/Thoth.Fetch.2.0.0/Fetch.fs.js";
import { Types_HttpRequestHeaders } from "./.fable/Fable.Fetch.2.1.0/Fetch.fs.js";
import { singleton } from "./.fable/fable-library.3.1.11/List.js";
import { CartModel$reflection } from "./MyStore.Dto/Shop.js";
import { obj_type } from "./.fable/fable-library.3.1.11/Reflection.js";
import { Product_ToDomain_Z6B097A11, Cart_ToDomain_268DEFC0 } from "./MyStore.Domain/Shop.js";
import { map } from "./.fable/fable-library.3.1.11/Array.js";
import { useFeliz_React__React_useState_Static_1505 } from "./.fable/Feliz.1.45.0/React.fs.js";
import { useFeliz_React__React_useDeferredCallback_Static_7088D81D, Deferred$1 } from "./.fable/Feliz.UseDeferred.1.4.1/UseDeferred.fs.js";
import { createElement } from "react";
import { map as map_1, singleton as singleton_1, append, delay, toList } from "./.fable/fable-library.3.1.11/Seq.js";
import { parse } from "./.fable/fable-library.3.1.11/Int32.js";
import { Interop_reactApi } from "./.fable/Feliz.1.45.0/Interop.fs.js";

export const Nullable_IntDecoder = (path) => ((v) => object((get$) => toNullable(get$.Optional.Raw(uncurry(2, int))), path, v));

export function Nullable_IntEncoder(nullable) {
    return option_1((value) => value)(ofNullable(nullable));
}

export const extra = new ExtraCoders((() => {
    let copyOfStruct = newGuid();
    return copyOfStruct;
})(), add("System.Nullable`1[System.Int32]", [(nullable) => Nullable_IntEncoder(nullable), Nullable_IntDecoder], empty.Coders));

export function getCartById(id) {
    return awaitPromise(PromiseBuilder__Run_212F1D4B(promise, PromiseBuilder__Delay_62FBFDE1(promise, () => {
        const url = toText(interpolate("https://localhost:5001/Cart/%i%P()", [id]));
        return Fetch_get_5760677E(url, void 0, void 0, singleton(new Types_HttpRequestHeaders(0, "application/json")), new CaseStrategy(1), extra, uncurry(2, void 0), {
            ResolveType: CartModel$reflection,
        }, {
            ResolveType: () => obj_type,
        });
    })));
}

export function Cart(cart) {
    const modelToDomain = (model) => [Cart_ToDomain_268DEFC0(cart.cart), map((arg00_1) => Product_ToDomain_Z6B097A11(arg00_1), cart.products)];
    const patternInput = useFeliz_React__React_useState_Static_1505(modelToDomain(cart));
    const products = patternInput[0][1];
    const cart_1 = patternInput[0][0];
    const patternInput_1 = useFeliz_React__React_useState_Static_1505(new Deferred$1(0));
    const loginState = patternInput_1[0];
    const patternInput_2 = useFeliz_React__React_useState_Static_1505(cart_1.CartId);
    const startLogin = useFeliz_React__React_useDeferredCallback_Static_7088D81D(() => getCartById(patternInput_2[0]), patternInput_1[1]);
    let y;
    switch (loginState.tag) {
        case 1: {
            y = createElement("i", {
                children: ["InProgress"],
            });
            break;
        }
        case 3: {
            const value_2 = loginState.fields[0].message;
            y = createElement("div", {
                children: [value_2],
            });
            break;
        }
        case 2: {
            const content = loginState.fields[0];
            patternInput[1](modelToDomain(content));
            const value_3 = toText(interpolate("Resolved%A%P()", [content]));
            y = createElement("div", {
                children: [value_3],
            });
            break;
        }
        default: {
            y = createElement("p", {
                children: ["HasNotStartedYet"],
            });
        }
    }
    console.log(some(cart_1));
    console.log(some(products));
    const c = toText(printf("%A"))(cart_1);
    const p = toText(printf("%A"))(products);
    const children = toList(delay(() => append(singleton_1(y), delay(() => append(singleton_1(createElement("input", {
        onChange: (ev) => {
            patternInput_2[1](parse(ev.target.value, 511, false, 32));
        },
    })), delay(() => append(singleton_1(createElement("button", {
        onClick: (t) => {
            startLogin();
        },
    })), delay(() => append(singleton_1(createElement("p", {
        children: [c],
    })), delay(() => append(singleton_1(createElement("p", {
        children: [p],
    })), delay(() => map_1((i) => createElement("p", {
        children: [i.Description],
    }), products)))))))))))));
    return createElement("div", {
        children: Interop_reactApi.Children.toArray(Array.from(children)),
    });
}

