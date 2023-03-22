import {createRouter, createWebHistory} from 'vue-router'
import routes from './routes'
import axios from 'axios'

const router = createRouter({
  history: createWebHistory(),
  routes: routes,
})

router.beforeEach((to, from, next) => {
  axios.get('https://localhost:7243/api/Member/id', {withCredentials: true,})
  .then((response) => {
    const isLoggedIn =response.data >0;
    const requiresAuth = to.matched.some(record => record.meta.requiresAuth); // 檢查是否需要登入才能訪問
  if (requiresAuth && !isLoggedIn) {
    // 需要登入但是使用者未登入，導航到登入頁面
    next('/login');
  } else {
    // 其他情況，直接繼續執行
    next();
  }
  })
  .catch((error) => {
    console.error(error);
  });

});

export default router


