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
    };
}

let model: IHomeModel;

export function init(params: IHomeModel) {
    model = params;
}




