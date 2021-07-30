import './vendor.min.js';
var popperInstance = null;
function showHideArrow(gtourWrapper, isHidden) {
    const arrowElement = gtourWrapper.querySelector('[data-popper-arrow]');
    if (arrowElement !== undefined && arrowElement !== null) {
        if (isHidden) {
            arrowElement.classList.add('force-hide');
        }
        else {
            arrowElement.classList.remove('force-hide');
        }
    }
}
export function SetTourStepPopperByElement(forElement, gtourWrapper, placement, strategy) {
    if (popperInstance !== null) {
        popperInstance.destroy();
    }
    if (forElement !== null) {
        showHideArrow(gtourWrapper, false);
        popperInstance = Popper.createPopper(forElement, gtourWrapper, {
            placement: placement,
            strategy: strategy
        });
    }
}
export function SetTourStepPopperBySelector(forElementSelector, gtourWrapper, placement, strategy) {
    const forElement = document.querySelector(forElementSelector);
    if (forElement !== undefined && forElement !== null) {
        SetTourStepPopperByElement(forElement, gtourWrapper, placement, strategy);
    }
}
export function ResetTourStepPopper(gtourWrapper, placement, strategy) {
    showHideArrow(gtourWrapper, true);
    SetTourStepPopperByElement(null, gtourWrapper, placement, strategy);
}
