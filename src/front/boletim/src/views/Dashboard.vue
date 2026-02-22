<template>
  <div>
    <h2>Lista de Posts</h2>
    <p v-if="carregando">Carregando dados da API</p>
    <template v-else>
      <div v-for="item in igrejas" :key="item.id">
        <IgrejaItem :item="item" />
      </div>
    </template>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { api } from '@/services/api'
import type Igreja from '@/types/Igreja'
import IgrejaItem from '@/components/IgrejaItem.vue'

const igrejas = ref<Igreja[]>([])
const carregando = ref(true)

onMounted(async () => {
  try {
    // A requisição já aponta para a Base URL e passa o Header correto
    igrejas.value = await api.get<Igreja[]>('/igreja/listar')
  } catch (erro) {
    alert('Erro ao carregar os dados. Verifique o console.')
  } finally {
    carregando.value = false
  }
})
</script>
