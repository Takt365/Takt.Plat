'use strict';
const fs = require('fs');
const path = require('path');
const root = path.join(__dirname, '..', 'frontend', 'src');
const files = [
  'views/logistics/manufacturing/engineering-change/ec/index.vue',
  'views/logistics/manufacturing/engineering-change/ec-attachment/index.vue',
  'views/logistics/manufacturing/engineering-change/ec/components/source-ec-input.vue',
  'views/logistics/manufacturing/engineering-change/ec/composables/use-ec-master-context.ts',
  'views/logistics/manufacturing/engineering-change/ec-attachment/composables/use-ec-master-context.ts',
  'views/logistics/manufacturing/engineering-change/ec-detail/composables/use-ec-master-context.ts',
  'views/logistics/manufacturing/engineering-change/ec/components/ec-form.vue',
  'views/logistics/manufacturing/engineering-change/ec-attachment/components/ec-form.vue',
  'views/logistics/manufacturing/engineering-change/ec-detail/components/ec-form.vue',
  'views/logistics/manufacturing/engineering-change/ec/components/ec-detail-panel.vue',
  'views/logistics/manufacturing/engineering-change/ec-attachment/components/ec-attachment-panel.vue',
  'views/dashboard/data-board/modules/StatsChangeModule.vue',
];
const reps = [
  ["from '@/api/logistics/manufacturing/engineering-change/ec'", "from '@/api/logistics/manufacturing/engineering-change/ec-gijutsu'"],
  ["from '@/types/logistics/manufacturing/engineering-change/ec'", "from '@/types/logistics/manufacturing/engineering-change/ec-gijutsu'"],
  ["from '@/types/logistics/manufacturing/engineering-change/ec-source-input'", "from '@/types/logistics/manufacturing/engineering-change/ec-gijutsu-source-input'"],
  ['getUnimportedSourceEcList', 'getUnimportedSourceEcGijutsuList'],
  ['importEcFromSource', 'importEcGijutsuFromSource'],
  ['updateEcStatus', 'updateEcGijutsuStatus'],
  ['deleteEcById', 'deleteEcGijutsuById'],
  ['deleteEcBatch', 'deleteEcGijutsuBatch'],
  ['getEcTemplate', 'getEcGijutsuTemplate'],
  ['getEcById', 'getEcGijutsuById'],
  ['getEcList', 'getEcGijutsuList'],
  ['createEc(', 'createEcGijutsu('],
  ['updateEc(', 'updateEcGijutsu('],
  ['importEc(', 'importEcGijutsu('],
  ['exportEc(', 'exportEcGijutsu('],
  ['getEcStat', 'getEcGijutsuStat'],
  ['EcSourceEcInputItem', 'EcGijutsuSourceEcInputItem'],
  ['EcSourceEcInputQuery', 'EcGijutsuSourceEcInputQuery'],
  ['EcImportFromSourceResult', 'EcGijutsuImportFromSourceResult'],
  ['EcImportFromSource', 'EcGijutsuImportFromSource'],
  ['EcStatQuery', 'EcGijutsuStatQuery'],
  ['EcStat', 'EcGijutsuStat'],
  ['EcQuery', 'EcGijutsuQuery'],
  ['EcCreate', 'EcGijutsuCreate'],
  ['EcUpdate', 'EcGijutsuUpdate'],
  ['EcStatus', 'EcGijutsuStatus'],
  ["import type { Ec,", "import type { EcGijutsu,"],
  ["import type { Ec }", "import type { EcGijutsu }"],
  ['Ref<Ec |', 'Ref<EcGijutsu |'],
  ['ref<Ec |', 'ref<EcGijutsu |'],
  ['Partial<EcGijutsuCreate & { ecId', 'Partial<EcGijutsuCreate & { ecGijutsuId'],
  ['formData?.ecId', 'formData?.ecGijutsuId'],
  ['props.formData?.ecId', 'props.formData?.ecGijutsuId'],
  ['val?.ecId', 'val?.ecGijutsuId'],
  ['master-id-column-key="ecId"', 'master-id-column-key="ecGijutsuId"'],
  [":id-column-key=\"'ecId'\"", ":id-column-key=\"'ecGijutsuId'\""],
  ["const entityIdName = 'ecId'", "const entityIdName = 'ecGijutsuId'"],
  ["dataIndex: 'ecId'", "dataIndex: 'ecGijutsuId'"],
  ["key: 'ecId'", "key: 'ecGijutsuId'"],
  ["getEcField(record, 'ecId')", "getEcField(record, 'ecGijutsuId')"],
  ['selectedMasterRow.value?.ecId', 'selectedMasterRow.value?.ecGijutsuId'],
  ['masterEcId', 'masterEcGijutsuId'],
  ['TaktEcs/stat', 'TaktEcGijutsus/stat'],
];
for (const rel of files) {
  const fp = path.join(root, rel);
  if (!fs.existsSync(fp)) {
    console.log('skip', rel);
    continue;
  }
  let s = fs.readFileSync(fp, 'utf8');
  let changed = false;
  for (const [a, b] of reps) {
    if (s.includes(a)) {
      s = s.split(a).join(b);
      changed = true;
    }
  }
  if (changed) {
    fs.writeFileSync(fp, s);
    console.log('updated', rel);
  }
}
