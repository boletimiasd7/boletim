<template>
  <div class="post">
    <p>{{ item.id }}</p>
    <div>{{ item.nome }}</div>
  </div>
</template>

<script setup lang="ts">
import type Igreja from '@/types/Igreja'

interface Props {
  item: Igreja
}

const { item } = defineProps<Props>()
</script>

<style scoped>
.post {
  border-bottom: 1px solid #f5f5f5;
  margin-bottom: 1rem;
}

.post-details {
  display: flex;
}
</style>
