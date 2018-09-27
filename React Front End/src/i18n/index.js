import { englishMessages } from 'admin-on-rest';
import frenchMessages from 'aor-language-french';
import spanishMessages from 'aor-language-spanish';

import customFrenchMessages from './fr';
import customEnglishMessages from './en';
import customSpanishMessages from './es';


export default {
    fr: { ...frenchMessages, ...customFrenchMessages },
    en: { ...englishMessages, ...customEnglishMessages },
    sp: { ...spanishMessages, ...customSpanishMessages }
};
