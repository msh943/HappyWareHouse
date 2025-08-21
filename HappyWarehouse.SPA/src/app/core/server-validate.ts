import { FormGroup } from '@angular/forms';


export function applyServerErrors(form: FormGroup, err: any): void {

  Object.values(form.controls).forEach(c => {
    const e = { ...(c.errors || {}) };
    delete e['server'];
    c.setErrors(Object.keys(e).length ? e : null);
  });
  form.setErrors(null);

  const errorsBag =
    err?.error?.errors ??
    err?.errors ??
    null;

  if (errorsBag && typeof errorsBag === 'object') {
    for (const key of Object.keys(errorsBag)) {
      const messages: string[] = errorsBag[key];
      const control =
        form.get(key) ||                                   // exact key
        form.get(lowerFirst(key)) ||                       // Email -> email
        form.get(key.charAt(0).toLowerCase() + key.slice(1)); // RoleId -> roleId

      if (control) {
        const prev = control.errors || {};
        control.setErrors({ ...prev, server: messages.join(' ') });
      } else {
        // attach as form-level error if no matching control
        const prev = form.errors || {};
        const merged = prev['serverAll']
          ? `${prev['serverAll']} ${messages.join(' ')}`
          : messages.join(' ');
        form.setErrors({ ...prev, serverAll: merged });
      }
    }
  } else {
    // non-validation error
    const msg = err?.error?.title || err?.error?.message || err?.message || 'Request failed';
    form.setErrors({ ...(form.errors || {}), serverAll: msg });
  }
}

function lowerFirst(s: string) {
  return s ? s.charAt(0).toLowerCase() + s.slice(1) : s;
}
