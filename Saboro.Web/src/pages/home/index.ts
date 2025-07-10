import $ from "jquery";
import Toast from "components/toast";
import "styles/index.less";
import "./index.less";
import UIkit from "uikit";
import Icons from "uikiticonsjs";

UIkit.use(Icons);

export { Toast };

interface IHomeModel {
    urls: {
        index: string;
        getCadastro: string;
    };
    message: {
        error: string;
        success: string;
    };
}

let model: IHomeModel;

export function init(params: IHomeModel) {
    model = params;
    handlerMessage(model);
}

function handlerMessage(model: IHomeModel) {
    if (model.message.error) Toast.error(model.message.error);

    if (model.message.success) {
        Toast.success(model.message.success);
        model.message.success = ""; 
    }
}

export const getCadastroReceita = () => {
    $.get(`${model.urls.getCadastro}`, {})
        .done(() => {
            window.location.href = `${model.urls.getCadastro}`;
        })
        .fail((response) => {
            Toast.error(response.responseText);
        });
};
