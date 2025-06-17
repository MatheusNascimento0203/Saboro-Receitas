import $ from 'jquery';

export default class Form {

    static serializeObject(element: any, options?: { verifyEmpty: boolean }) {
        let form = $(element);

        if (!form.is('form')) {
            form = $('<form/>').append(form.clone());
        }

        const obj: any = {};
        const arr = form.serializeArray();

        for (let i = 0; i < arr.length; i++) {
            obj[arr[i].name] = arr[i].value;
        }

        form.find('.uk-form-controls').find('input, select, textarea').trigger('input');
        obj.isValid = !form.find('.uk-form-controls.error').length;

        if (!options)
            return obj;

        obj.length = arr.length;

        if (options.verifyEmpty) {
            let focused = false;
            for (const prop in obj) {
                if (!obj[prop].toString().trim()) {
                    obj.emptyLength = (obj.emptyLength || 0) + 1;

                    const input = form.find(`[name="${prop}"]`);
                    if (input.is('.required:visible')) {
                        obj.requiredEmptyLength = (obj.requiredEmptyLength || 0) + 1;

                        if (!focused) {
                            input.focus();
                            focused = true;
                        }
                    }
                }
            }
        }

        return obj;
    }
}