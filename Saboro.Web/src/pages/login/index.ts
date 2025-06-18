import $ from "jquery";
import Toast from "components/toast";
import "styles/index.less";
import "./index.less";
import UIkit from "uikit";
import Icons from "uikiticonsjs";

UIkit.use(Icons);

interface ILoginModel {
    urls: {
        index: string;
        enviarCriacaoUsuario: string;
        enviarRedefinirSenha: string;
        formCriarUsuario: string;        
    },
    message: {
        error: string;
        success: string;
    }
}

let model: ILoginModel;

export function init(params: ILoginModel) {
    model = params;
    handlerMessage(model);
}

function handlerMessage(model: ILoginModel) {

    if (model.message.error)
        Toast.error(model.message.error);

    if (model.message.success)
        Toast.success(model.message.success);
}

function formCriarUsuario() {
    
}