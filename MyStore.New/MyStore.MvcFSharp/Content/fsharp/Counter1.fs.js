import { singleton } from "./.fable/fable-library.3.1.11/AsyncBuilder.js";
import { sleep } from "./.fable/fable-library.3.1.11/Async.js";
import { useFeliz_React__React_useState_Static_1505 } from "./.fable/Feliz.1.45.0/React.fs.js";
import { useFeliz_React__React_useDeferred_Static_2344FC52 } from "./.fable/Feliz.UseDeferred.1.4.1/UseDeferred.fs.js";
import { createElement } from "react";
import { join } from "./.fable/fable-library.3.1.11/String.js";
import { ofArray } from "./.fable/fable-library.3.1.11/List.js";
import { Interop_reactApi } from "./.fable/Feliz.1.45.0/Interop.fs.js";

export const loadData = singleton.Delay(() => singleton.Bind(sleep(1000), () => singleton.Return("Hello!")));

export function Counter1(counter1InputProps) {
    const patternInput = useFeliz_React__React_useState_Static_1505(counter1InputProps.init);
    const setCount = patternInput[1];
    const count = patternInput[0] | 0;
    const data = useFeliz_React__React_useDeferred_Static_2344FC52(loadData, []);
    let data_1;
    switch (data.tag) {
        case 1: {
            data_1 = createElement("i", {
                className: join(" ", ["fa", "fa-refresh", "fa-spin", "fa-2x"]),
            });
            break;
        }
        case 3: {
            const value_1 = data.fields[0].message;
            data_1 = createElement("div", {
                children: [value_1],
            });
            break;
        }
        case 2: {
            data_1 = createElement("h1", {
                children: [data.fields[0]],
            });
            break;
        }
        default: {
            data_1 = null;
        }
    }
    const children = ofArray([createElement("h1", {
        children: [count],
    }), createElement("button", {
        onClick: (_arg1) => {
            setCount(count + 1);
        },
        children: "Increment",
    }), createElement("button", {
        onClick: (_arg2) => {
            setCount(count - 2);
        },
        children: "Decrement",
    }), data_1]);
    return createElement("div", {
        children: Interop_reactApi.Children.toArray(Array.from(children)),
    });
}

