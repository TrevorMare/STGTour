/// <reference types="popper.js" />

import './vendor.min.js';

var popperInstance: any = null;

function showHideArrow(gtourWrapper: HTMLElement, isHidden: boolean) {

  const arrowElement = gtourWrapper.querySelector('[data-popper-arrow]');
  if (arrowElement !== undefined && arrowElement !== null) {
    if (isHidden) {
      arrowElement.classList.add('force-hide');
    } else {
      arrowElement.classList.remove('force-hide');
    }
  }
}

export function SetTourStepPopperByElement(forElement: HTMLElement, gtourWrapper: HTMLElement, placement: string, strategy: string): void {

  if (popperInstance !== null) {
    popperInstance.destroy();
  }

  if (forElement !== null) {
    showHideArrow(gtourWrapper, false);
    // @ts-ignore 
    popperInstance = Popper.createPopper(forElement, gtourWrapper, {
      placement: placement,
      strategy: strategy
    });
  }
} 

export function SetTourStepPopperBySelector(forElementSelector: string, gtourWrapper: HTMLElement, placement: string, strategy: string): void {

  const forElement: HTMLElement = document.querySelector(forElementSelector);
  if (forElement !== undefined && forElement !== null) {
    SetTourStepPopperByElement(forElement, gtourWrapper, placement, strategy);
  }
}

export function ResetTourStepPopper(gtourWrapper: HTMLElement, placement: string, strategy: string): void {
  showHideArrow(gtourWrapper, true);
  SetTourStepPopperByElement(null, gtourWrapper, placement, strategy);
}