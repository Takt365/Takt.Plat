'use strict';

const fs = require('fs');
const path = require('path');

const file = path.join(
  __dirname,
  '../frontend/src/views/logistics/materials/material-plant/index.vue'
);
let s = fs.readFileSync(file, 'utf8');

// Remove second advanced-query materialDescription block (textarea)
s = s.replace(
  /      <div v-show="isFieldVisible\('materialDescription'\)">\r?\n      <a-form-item :label="t\('entity\.materialplant\.materialdescription'\)">\r?\n        <a-input\r?\n          v-model:value="advancedQueryForm\.materialDescription"\r?\n          :placeholder="t\('common\.page\.form\.placeholder\.required', \{ field: t\('entity\.materialplant\.materialdescription'\) \}\)"\r?\n          show-count\r?\n          :maxlength="40"\r?\n          allow-clear\r?\n        \/>\r?\n      <\/a-form-item>\r?\n      <\/div>\r?\n      <div v-show="isFieldVisible\('materialSpecification'\)">\r?\n      <a-form-item :label="t\('entity\.materialplant\.materialspecification'\)">\r?\n        <a-input\r?\n          v-model:value="advancedQueryForm\.materialSpecification"\r?\n          :placeholder="t\('common\.page\.form\.placeholder\.required', \{ field: t\('entity\.materialplant\.materialspecification'\) \}\)"\r?\n          show-count\r?\n          :maxlength="80"\r?\n          allow-clear\r?\n        \/>\r?\n      <\/a-form-item>\r?\n      <\/div>\r?\n      <div v-show="isFieldVisible\('materialDescription'\)">\r?\n      <a-form-item :label="t\('entity\.materialplant\.materialdescription'\)">\r?\n        <a-textarea\r?\n          v-model:value="advancedQueryForm\.materialDescription"\r?\n          :placeholder="t\('common\.page\.form\.placeholder\.optional', \{ field: t\('entity\.materialplant\.materialdescription'\) \}\)"\r?\n          :rows="2"\r?\n          allow-clear\r?\n        \/>\r?\n      <\/a-form-item>\r?\n      <\/div>\r?\n/,
  `      <div v-show="isFieldVisible('materialDescription')">
      <a-form-item :label="t('entity.materialplant.materialdescription')">
        <a-input
          v-model:value="advancedQueryForm.materialDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.materialplant.materialdescription') })"
          show-count
          :maxlength="80"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialSpecification')">
      <a-form-item :label="t('entity.materialplant.materialspecification')">
        <a-input
          v-model:value="advancedQueryForm.materialSpecification"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.materialplant.materialspecification') })"
          show-count
          :maxlength="80"
          allow-clear
        />
      </a-form-item>
      </div>
`
);

// Collapse duplicate object keys: materialDescription twice
s = s.replace(
  /  materialDescription: '',\r?\n  materialSpecification: '',\r?\n  materialDescription: '',\r?\n/g,
  `  materialDescription: '',\n  materialSpecification: '',\n`
);

// Duplicate column definitions
s = s.replace(
  /  \{\r?\n    title: t\('entity\.materialplant\.materialdescription'\),\r?\n    dataIndex: 'materialDescription',\r?\n    key: 'materialDescription',\r?\n    width: 120,\r?\n    resizable: true,\r?\n    ellipsis: true,\r?\n    customRender: \(\{ record \}: \{ record: any \}\) => getMaterialPlantField\(record, 'materialDescription'\) \?\? ''\r?\n  \},\r?\n  \{\r?\n    title: t\('entity\.materialplant\.materialspecification'\),\r?\n    dataIndex: 'materialSpecification',\r?\n    key: 'materialSpecification',\r?\n    width: 120,\r?\n    resizable: true,\r?\n    ellipsis: true,\r?\n    customRender: \(\{ record \}: \{ record: any \}\) => getMaterialPlantField\(record, 'materialSpecification'\) \?\? ''\r?\n  \},\r?\n  \{\r?\n    title: t\('entity\.materialplant\.materialdescription'\),\r?\n    dataIndex: 'materialDescription',\r?\n    key: 'materialDescription',\r?\n    width: 120,\r?\n    resizable: true,\r?\n    ellipsis: true,\r?\n    customRender: \(\{ record \}: \{ record: any \}\) => getMaterialPlantField\(record, 'materialDescription'\) \?\? ''\r?\n  \},\r?\n/,
  `  {
    title: t('entity.materialplant.materialdescription'),
    dataIndex: 'materialDescription',
    key: 'materialDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialPlantField(record, 'materialDescription') ?? ''
  },
  {
    title: t('entity.materialplant.materialspecification'),
    dataIndex: 'materialSpecification',
    key: 'materialSpecification',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialPlantField(record, 'materialSpecification') ?? ''
  },
`
);

// Duplicate advanced field options
s = s.replace(
  /  \{ key: 'materialDescription', label: t\('entity\.materialplant\.materialdescription'\) \},\r?\n  \{ key: 'materialSpecification', label: t\('entity\.materialplant\.materialspecification'\) \},\r?\n  \{ key: 'materialDescription', label: t\('entity\.materialplant\.materialdescription'\) \},\r?\n/g,
  `  { key: 'materialDescription', label: t('entity.materialplant.materialdescription') },\n  { key: 'materialSpecification', label: t('entity.materialplant.materialspecification') },\n`
);

// Duplicate assignTrimmed
s = s.replace(
  /  assignTrimmed\('materialDescription', form\.materialDescription\)\r?\n  assignTrimmed\('materialSpecification', form\.materialSpecification\)\r?\n  assignTrimmed\('materialDescription', form\.materialDescription\)\r?\n/g,
  `  assignTrimmed('materialDescription', form.materialDescription)\n  assignTrimmed('materialSpecification', form.materialSpecification)\n`
);

fs.writeFileSync(file, s);
console.log('index.vue cleaned', {
  descCount: (s.match(/materialDescription/g) || []).length,
});
