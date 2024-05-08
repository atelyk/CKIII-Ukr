# Crusader Kings III Українською

[![Crowdin](https://badges.crowdin.net/crusader-kings-iii-ukrainian/localized.svg)](https://crowdin.com/project/crusader-kings-iii-ukrainian)

Допоміжний проєкт для підтримки фанатського перекладу Crusader Kings III українською.

В разі якщо поточна команда не зможе продовжувати роботу, сподіваюсь цей проект допоможе послідовникам.

## Лінки

 - [Працюючий мод](https://steamcommunity.com/sharedfiles/filedetails/?id=3127700882)
 - [Проєкт перекладу](https://crowdin.com/project/crusader-kings-iii-ukrainian/uk)
 - [Діскорд](https://discord.gg/uUXe67aJnz)
 
 
## Інструменти
* Консольний додаток для:
    * конвертації оригінальних файлів гри в підходящий для Crowdin формат (коректний yml)
    * діставання з гри файлів що необхідні для локалізації (при оновленні версії гри)
    * для зворотнього форматування (cхоже не обов'язковий етап)
* Файл конфігурації для Crowdin


## Робота з Crowdin

### При оновленні версії гри:
1. Скачати останні переклади з проекту і зберегти окремо (читай нижче як саме)
2. Запустити CK3-format-converter
3. Згодувати йому папку з грою
4. Обрати опцію "дістати файли гри"
5. Знайти новостворену папку, що починається з "for_translation_" з ієрархією файлів
6. Запустити CK3-format-converter для цієї нової папки з опцією "корректний формат" (дуже важливо!)
7. Знайти новостворену папку, що починається з "corrected_for_translation_"
8. Покласти оновлені файлі в папку localization/en/ з заміною
9. Запустити консольний додаток з наступною командою:

```bash
  crowdin upload sources -v --auto-update --preserve-hierarchy
```

### Для оновлення перекладів 
В разі якщо є окремі файлі, або групи файлів.
1. Запустити CK3-format-converter для корекції формату
2. Покласти в папку localization/uk/ зі збереженням ієрархії (!)
3. Щоб процес займав менше часу, існуючі файли можна видалити.

```bash
  crowdin upload translations -l uk --no-auto-approve-imported --no-import-eq-suggestions -v --preserve-hierarchy
```

### Для скачування останньої версії перекладу
1. Запустити консоль в папці проекту з командою:
```bash
  crowdin download -v
```