import type { UserConfig } from '@commitlint/types'

const config: UserConfig = {
  extends: ['@commitlint/config-conventional'],
  rules: {
    'type-enum': [
      2, 'always',
      ['feat', 'fix', 'docs', 'style', 'refactor', 'test', 'chore', 'perf', 'ci', 'build', 'revert'],
    ],
    'scope-enum': [
      2, 'always',
      ['api', 'frontend', 'identity', 'members', 'shared', 'docker', 'ci', 'deps', 'docs', 'auth', 'roles', 'users', 'permissions', 'courses'],
    ],
    'scope-empty': [2, 'never'],
    'subject-case': [2, 'always', 'lower-case'],
    'subject-max-length': [2, 'always', 72],
    'body-max-line-length': [2, 'always', 100],
  },
}

export default config
