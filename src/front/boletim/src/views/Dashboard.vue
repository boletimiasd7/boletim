<template>
  <div>
    <h2>Lista de Posts</h2>
    <p v-if="carregando">Carregando dados da API</p>
    <template v-else>
      <div v-for="item in posts" :key="item.id">
        <PostItem :item="item" />
      </div>
    </template>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { api } from '@/services/api';
import type Post from '@/types/Post';
import PostItem from '@/components/PostItem.vue';


const posts = ref<Post[]>([]);
const carregando = ref(true);

onMounted(async () => {
  try {
    // A requisição já aponta para a Base URL e passa o Header correto
    posts.value = await api.get<Post[]>('/posts');
  } catch (erro) {
    alert('Erro ao carregar os dados. Verifique o console.');
  } finally {
    carregando.value = false;
  }
});
</script>
