<template>
    <div>
        <embed v-bind:src="urlBoletim" type="application/pdf" width="100%" height="600px" />
    </div>
</template>
<script setup lang="ts">
import { api } from '@/services/api';
import { ref, onMounted } from 'vue'

let urlBoletim = ref<string>('');

onMounted(async() => {
    urlBoletim.value = api.montarUrl('/boletim/gerarpdf');
});

</script>