import $ from 'jquery';
import UIkit from 'uikit';

export default class Loading {
    private static overlayElement: JQuery<HTMLElement> | null = null;
    private static styleElement: JQuery<HTMLElement> | null = null;
    private static isVisible: boolean = false;
    private static hideTimeoutId: number | null = null;

    static show(options: {
        mensagem?: string;
        backgroundColor?: string;
        spinnerColor?: string;
        spinnerSize?: number;
    } = {}) {

        if (this.isVisible) return;
        
        if (this.hideTimeoutId !== null) {
            window.clearTimeout(this.hideTimeoutId);
            this.hideTimeoutId = null;
        }
        $('body > .uk-overlay.uk-position-fixed').remove();
        
        const mensagem = options.mensagem || '';
        const backgroundColor = options.backgroundColor || 'rgba(0, 0, 0, 0.5)';
        const spinnerColor = options.spinnerColor || '#fff';
        const spinnerSize = options.spinnerSize || 4.5;

        this.overlayElement = $(`<div class="uk-overlay uk-position-fixed uk-position-cover" style="background-color: ${backgroundColor}; z-index: 9999;"></div>`);
        
        const spinnerHtml = `        
            <div class="uk-flex uk-flex-column uk-flex-middle uk-position-center uk-text-center">
                <span uk-spinner="ratio: ${spinnerSize}" style="color: ${spinnerColor};"></span>
                <span class="uk-margin-small-top text-white">${mensagem}</span>
            </div>            
        `;
        
        this.overlayElement.append(spinnerHtml);
        $('body').append(this.overlayElement);
        $('body').css('overflow', 'hidden');
        
        this.isVisible = true;
        this.hideTimeoutId = window.setTimeout(() => {
            console.warn('Loading foi escondido pelo timeout de segurança');
            this.hide();
        }, 10000);
    }

    static hide() {
        if (this.hideTimeoutId !== null) {
            window.clearTimeout(this.hideTimeoutId);
            this.hideTimeoutId = null;
        }
        
        this.hideTimeoutId = window.setTimeout(() => {
            $('body > .uk-overlay.uk-position-fixed').remove();
            this.overlayElement = null;
            
            if (this.styleElement) {
                this.styleElement.remove();
                this.styleElement = null;
            }
            $('body').css('overflow', '');
            
            this.isVisible = false;
            this.hideTimeoutId = null;
        }, 500);
    }

    static async during<T>(asyncFn: () => Promise<T>, options = {}): Promise<T> {
        try {
            this.show(options);
            return await asyncFn();
        } finally {
            this.hide();
        }
    }

    static disableForm(form: HTMLFormElement | JQuery<HTMLFormElement>, disable: boolean = true) {
        $(form).find('input, button, a').prop('disabled', disable);
    }
}